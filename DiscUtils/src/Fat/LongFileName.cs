using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace DiscUtils.Fat
{

    /// <summary>
    /// Represents a long file name.
    /// </summary>
    internal class LongFileName
    {
        private readonly string _filename;
        private static readonly char[] SpacePeriod = new char[] { ' ', '.' };

        /// <summary>
        /// Initializes a new instance of the <see cref="LongFileName"/> class.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <exception cref="System.ArgumentNullException">filename</exception>
        /// <exception cref="System.ArgumentException">filename</exception>
        public LongFileName(string filename)
        {
            if (filename == null) throw new ArgumentNullException("filename");
            if (filename.Length == 0) throw new ArgumentException("filename");

            _filename = filename;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="LongFileName"/> class from being created.
        /// </summary>
        private LongFileName()
        {
            _filename = string.Empty;
        }

        /// <summary>
        /// Gets the filename.
        /// </summary>
        /// <value>
        /// The filename.
        /// </value>
        public string Filename { get { return _filename; } }

        /// <summary>
        /// Represents the empty filename. This field is read-only.
        /// </summary>
        public static LongFileName Empty = new LongFileName();

        /// <summary>
        /// Determines whether the specified filename is valid.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="filename"/> is null.
        /// </exception>
        public static bool IsValid(string filename)
        {
            if (filename == null) throw new ArgumentNullException("filename");

            if (filename.Length == 0 || filename.Length > 255)
                return false;

            foreach (var c in filename)
            {
                if (IsForbidden(c))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// To the short name.
        /// </summary>
        /// <param name="fileExists">
        /// The function that will be called to check if a file with 
        /// the same name already exists in the current directory.
        /// This function should return <c>true</c> if the file 
        /// already exists. If the file does not already exist, 
        /// return <c>false</c>. The function will be passed the 
        /// potentital 8.3 short name.
        /// </param>
        /// <returns></returns>
        public string ToShortName(Func<string, bool> fileExists)
        {
            /* Both FAT and NTFS use the Unicode character set for their names,
            * which contain several forbidden characters that MS-DOS cannot 
            * read in any file name. To generate a short MS-DOS-readable file 
            * name for a file, Windows 2000 deletes all of these characters 
            * from the long file name and removes any spaces. Because an 
            * MS-DOS-readable file name can have only one period, Windows 
            * 2000 also removes all extra periods from the file name. 
            * Next, Windows 2000 truncates the file name, if necessary, 
            * to six characters and appends a tilde ( ~ ) and a number. 
            * For example, each nonduplicate file name is appended with ~1 . 
            * Duplicate file names end with ~2 , then ~3 , and so on. 
            * After the file names are truncated, the file name 
            * extensions are truncated to three or fewer characters. 
            * Finally, when displaying file names at the command line, 
            * Windows 2000 translates all characters in the file name 
            * and extension to uppercase.
            */

            /* When there are five or more files that would result in 
             * duplicate short file names, Windows 2000 uses a slightly 
             * different method for creating short file names. For the 
             * fifth and subsequent files, Windows 2000:
             * 
             *  - Uses only the first two letters of the long file name.
             *  - Generates the next four letters of the short file name by 
             *    mathematically manipulating the remaining letters of the 
             *    long file name.
             *  - Appends ~1 (or another number, if necessary, to avoid a 
             *    duplicate file name) to the result.
             */

            // remove any trailing spaces or periods
            string filename = _filename.TrimEnd(SpacePeriod);

            if (!IsLongFileName(filename))
            {
                return filename.ToUpper();
            }

            string shortName;

            if (TryFirstFourNames(filename, fileExists, out shortName))
            {
                return shortName;
            }

            if (TryHashRemaining(filename, fileExists, out shortName, GetHash))
            {
                return shortName;
            }

            while (!TryHashRemaining(filename, fileExists, out shortName, GetRandomHash))
            {
                // the fileExists function better return false soon or this could loop forever
            }

            return shortName;
 
        }

        public static bool IsLongFileName(string filename)
        {
            return IsLongName(filename) || IsLongExtension(filename);
        }

        private static bool IsLongName(string filename)
        {
            string name = Path.GetFileNameWithoutExtension(filename);

            return !string.IsNullOrEmpty(name) && (8 < name.Length || name.EndsWith(" "));
        }

        private static bool IsLongExtension(string filename)
        {
            string extension = Path.GetExtension(filename);
            return !string.IsNullOrEmpty(extension) && 4 < extension.Length;
        }
        
        /// <summary>
        /// Try to create a short name based on the first 6 valid letters
        /// of the file name followed by ~1 / ~2 / ~3 or ~4.
        /// </summary>
        /// <param name="filename">The long file name</param>
        /// <param name="fileExists">
        /// A function that will be called to determine if the a file with the same name already exists.</param>
        /// <param name="shortName">The unique short file name.</param>
        /// <returns>
        /// <c>true</c> if a unique short name was found.
        /// </returns>
        private static bool TryFirstFourNames(string filename, Func<string, bool> fileExists, out string shortName)
        {
            string extension = FixExtension(Path.GetExtension(filename));
            string name = Path.GetFileNameWithoutExtension(filename);

            int lastPeriod = name.LastIndexOf('.');
            StringBuilder shortNameBuilder = new StringBuilder();

            for (int i = 0; i < name.Length && shortNameBuilder.Length < 6; i++)
            {
                var c = name[i];
                if (IsForbidden(c) || (i != lastPeriod && c == '.'))
                {
                    continue;
                }

                shortNameBuilder.Append(Char.ToUpper(c));
            }

            shortNameBuilder.Append("~{0}");
            shortNameBuilder.Append(extension);

            string shortNameFormat = shortNameBuilder.ToString();
            return CheckExists(shortNameFormat, fileExists, 4, out shortName);
        }

        /// <summary>
        /// Try to create a short name based on the first 2 valid letters
        /// of the file name followed by the hash with length of 4.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="fileExists">The file exists.</param>
        /// <param name="shortName">The short name.</param>
        /// <param name="hashFunc">The hash function.</param>
        /// <returns></returns>
        private static bool TryHashRemaining(string filename, Func<string, bool> fileExists, out string shortName, Func<string, string> hashFunc)
        {
            string name = Path.GetFileNameWithoutExtension(filename);
            string extension = FixExtension(Path.GetExtension(filename));

            var shortNameBuilder = new StringBuilder();

            for (int i = 0; i < name.Length && shortNameBuilder.Length < 2; i++)
            {
                var c = name[i];
                if (IsForbidden(c))
                {
                    continue;
                }

                shortNameBuilder.Append(Char.ToUpper(c));
            }

            shortNameBuilder.Append(hashFunc(name));
            shortNameBuilder.Append("~{0}");
            shortNameBuilder.Append(extension);

            string shortNameFormat = shortNameBuilder.ToString();

            return CheckExists(shortNameFormat, fileExists, 9, out shortName);
        }

        private static bool CheckExists(string shortNameFormat, Func<string, bool> fileExists, int maxIndex, out string shortName)
        {
            for (int i = 1; i <= maxIndex; i++)
            {
                string nextShortName = string.Format(shortNameFormat, i);

                if (!fileExists(nextShortName))
                {
                    shortName = nextShortName;
                    return true;
                }
            }

            shortName = null;
            return false;
        }
        
        private static bool IsForbidden(char c)
        {
            // short circut on known good characters
            if (('A' <= c && c <= 'Z') || ('a' <= c && c <= 'z') || ('0' <= c && c <= '9'))
                return false;

            // short circut on known bad characters
            if (c == ' ' || c == '<' || c == '>' || c == '*' || c == ':' || c == '?' || c == '/' || c == '\\')
                return true;

            foreach (var invalidFileNameChar in Path.GetInvalidFileNameChars())
            {
                if (c == invalidFileNameChar)
                {
                    return true;
                }
            }

            return false;
        }

        private static string FixExtension(string extension)
        {
            if (string.IsNullOrEmpty(extension))
                return string.Empty;

            Debug.Assert(extension[0] == '.');

            StringBuilder extensionBuilder = new StringBuilder();
            extensionBuilder.Append('.');

            for (int i = 1; i < extension.Length && extensionBuilder.Length < 4; i++)
            {
                var c = extension[i];
                Debug.Assert(c != '.');

                if (IsForbidden(c))
                {
                    continue;
                }

                extensionBuilder.Append(Char.ToUpper(c));
            }

            return extensionBuilder.ToString();
        }

        /// <summary>
        /// Gets the hash value for the given name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        private static string GetHash(string name)
        {
            // hashes need to be case insensitive
            byte[] hash = BitConverter.GetBytes(name.ToUpper().GetHashCode());

            // reduce the 4 bytes (8 hex digits) down to 2 bytes (4 hex) digits.
            byte[] smallHash = new byte[2];
            smallHash[0] = (byte)(hash[0] ^ hash[2]);
            smallHash[1] = (byte)(hash[1] ^ hash[3]);

            var hex = ToHex(smallHash);
            return hex;
        }


        // http://stackoverflow.com/questions/623104/byte-to-hex-string/3974535#3974535
        private static string ToHex(byte[] bytes)
        {
            char[] c = new char[bytes.Length * 2];

            byte b;

            for (int bx = 0, cx = 0; bx < bytes.Length; ++bx, ++cx)
            {
                b = ((byte)(bytes[bx] >> 4));
                c[cx] = (char)(b > 9 ? b + 0x37 : b + 0x30);

                b = ((byte)(bytes[bx] & 0x0F));
                c[++cx] = (char)(b > 9 ? b + 0x37 : b + 0x30);
            }

            return new string(c);
        }

        /// <summary>
        /// Gets a random hash value using a random number generator.
        /// </summary>
        /// <param name="ignore">Dummy parameter so signature matches <see cref="GetHash"/>.</param>
        /// <returns></returns>
        private static string GetRandomHash(string ignore)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            byte[] buffer = new byte[2];
            random.NextBytes(buffer);

            return ToHex(buffer);
        }
    }

    /// <summary>
    /// Responsible for:
    /// - reading the stream
    /// </summary>
    internal static class DirectoryEntryReader
    {
        private const int EntrySize = 32;

        private const byte DeletedEntry = 0xe5;

        /// <summary>
        /// Reads one or more directory entries from the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">stream</exception>
        /// <exception cref="System.InvalidOperationException">Stream is not readable.</exception>
        public static byte[] Read(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            if (!stream.CanRead)
            {
                throw new InvalidOperationException("Stream is not readable.");
            }

            byte[] buffer = ReadOneEntry(stream);

            // buffer could contain a long or short entry
            // if the entry is deleted
            if (!IsDeletedEntry(buffer) && IsLongEntry(buffer))
            {
                return ReadLongEntry(buffer, stream);
            }

            return buffer;
        }

        private static bool IsDeletedEntry(byte[] buffer)
        {
            Debug.Assert(buffer != null);
            Debug.Assert(buffer.Length == EntrySize);

            return buffer[0] == DeletedEntry;
        }

        private static byte[] ReadOneEntry(Stream stream)
        {
            Debug.Assert(stream != null);

            return Utilities.ReadFully(stream, EntrySize);
        }

        private static bool IsLongEntry(byte[] buffer)
        {
            Debug.Assert(buffer != null);
            Debug.Assert(buffer.Length == EntrySize);

            return LongDirectoryEntry.Attributes == LongDirectoryEntry.GetAttributes(buffer);
        }

        /// <summary>
        /// Reads all of the directory entries that make up long directory entry 
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        /// <remarks>
        /// 
        ///   [ part  n   ]
        ///   [     n - 1 ]
        ///   [     n - 2 ]
        ///   [      ...  ]
        ///   [       1   ]
        ///   [      fat  ]
        /// </remarks>
        private static byte[] ReadLongEntry(byte[] entry, Stream stream)
        {
            Debug.Assert(entry != null);
            Debug.Assert(entry.Length == EntrySize);

            if ((entry[0] & LongDirectoryEntry.Masks.LastLongFileNameEntry) == 0)
            {
                throw new IOException("Expected to be on last entry.");
            }

            // get the number of vfat parts
            int n = entry[0] & LongDirectoryEntry.Masks.LongFileNameEntryNumber;

            // allocate memory for all of the vfat entries plus the fat entry
            var segments = CreateSegments(n + 1);

            // the vfat entries are stored in decscending order (last one first) ie,  n, n-1, n-2, ..., 1
            // store each of the vfat entries in logical order: 1, 2, ... n
            BlockCopy(entry, segments[0]);

            // by reading 1..n, the fat entry will be read also
            for (int i = 1; i <= n; i++)
            {
                entry = ReadOneEntry(stream);
                BlockCopy(entry, segments[i]);
            }

            return segments[0].Array;
        }

        private static void BlockCopy(byte[] source, ArraySegment<byte> destination)
        {
            System.Buffer.BlockCopy(source, 0, destination.Array, destination.Offset, source.Length);
        }

        /// <summary>
        /// Creates the specified number of segments. All of the segments will share the same underlying buffer.
        /// </summary>
        /// <param name="n">The number of segments to create.</param>
        /// <returns></returns>
        private static ArraySegment<byte>[] CreateSegments(int n)
        {
            byte[] entries = new byte[n * EntrySize];

            ArraySegment<byte>[] segments = new ArraySegment<byte>[n];
            for (int offset = 0, i = 0; offset < entries.Length; offset += EntrySize, i++)
            {
                segments[i] = new ArraySegment<byte>(entries, offset, EntrySize);
            }

            return segments;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class DirectoryEntryEncoding
    {
        public byte[] GetBytes(DirectoryEntry entry, Action<ArraySegment<byte>> writeToAction)
        {
            int entryCount = entry.EntryCount;
            ArraySegment<byte>[] entries = CreateEmptyEntries(entry.EntryCount);

            if (entry.Name.HasLongFilename)
            {
                var encoding = new UnicodeEncoding();
                string longName = entry.Name.GetLongName();
                bool isDeleted = entry.Name.IsDeleted();
                var filename = encoding.GetBytes(PadLongFilename(longName));

                // if the the entry is deleted, the checksum will be wrong
                // but that does not matter as deleted entries are skipped
                var checksum = GetChecksum(entry);

                for (int i = 0; i < entryCount - 1; i++)
                {
                    // 3, 2, 1
                    int ordinal = (entryCount - i - 1);

                    EncodeEntry(entries[i], ordinal, filename, checksum, i == 0, isDeleted);
                }
            }

            // call back to the directory entry to write it's data to the last segment
            writeToAction(entries[entries.Length - 1]);

            return entries[0].Array;
        }

        public DirectoryEntry GetDirectoryEntry(FatFileSystemOptions options, Stream stream)
        {
            byte[] bytes = DirectoryEntryReader.Read(stream);

            ArraySegment<byte>[] directoryEntries = CreateEmptyEntries(bytes);

            string longFilename = string.Empty;

            if (1 < directoryEntries.Length)
            {
                longFilename = ReadLongFilename(directoryEntries);
            }

            int lastIndex = directoryEntries.Length - 1;
            DirectoryEntry directoryEntry = DirectoryEntry.CreateFrom(options, directoryEntries[lastIndex], longFilename);
            return directoryEntry;

        }


        private static string ReadLongFilename(ArraySegment<byte>[] directoryEntries)
        {
            Encoding encoding = new UnicodeEncoding();

            var filenameBuilder = new StringBuilder(256); // file names cannot be longer than 255 characters

            for (int index = directoryEntries.Length - 2; index >= 0; index--)
            {
                var directoryEntry = directoryEntries[index];

                var name0_4 = encoding.GetChars(directoryEntry.Array, directoryEntry.Offset + 1, 5 * 2);
                var name5_10 = encoding.GetChars(directoryEntry.Array, directoryEntry.Offset + 14, 6 * 2);
                var name11_12 = encoding.GetChars(directoryEntry.Array, directoryEntry.Offset + 28, 2 * 2);

                filenameBuilder.Append(name0_4);
                filenameBuilder.Append(name5_10);
                filenameBuilder.Append(name11_12);
            }

            string beforeTrim = filenameBuilder.ToString();

            TrimLongFilename(filenameBuilder);

            string afterTrim = filenameBuilder.ToString();

            string afterPad = PadLongFilename(afterTrim);

            Debug.Assert(beforeTrim == afterPad);

            string filename = filenameBuilder.ToString();
            return filename;
        }

        private static void TrimLongFilename(StringBuilder name)
        {
            while (name.Length > 0)
            {
                int end = name.Length - 1;

                if (name[end] == '\0' || name[end] == '\uffff')
                {
                    name.Remove(end, 1);
                }
                else
                {
                    break;
                }
            }
        }

        private static string PadLongFilename(string name)
        {
            if (name.Length % 13 == 0)
                return name;

            int padLength = (13 - name.Length % 13);

            StringBuilder builder = new StringBuilder(name, name.Length + padLength);
            builder.Append('\0');

            while (--padLength > 0)
            {
                builder.Append('\uffff');
            }

            return builder.ToString();
        }

        public static byte FilenameChecksum(ArraySegment<byte> segment)
        {
            var buffer = segment.Array;
            var offset = segment.Offset;
            return FilenameChecksum(buffer, offset);
        }

        private static byte FilenameChecksum(byte[] buffer, int offset)
        {
            byte sum = 0;

            for (int i = 0; i < 11; i++)
            {
                sum = (byte)((((sum & 1) << 7) | ((sum & 0xfe) >> 1)) + buffer[i + offset]);
            }

            return sum;
        }


        private ArraySegment<byte>[] CreateEmptyEntries(int count)
        {
            byte[] buffer = new byte[count * 32];

            ArraySegment<byte>[] entries = new ArraySegment<byte>[count];
            for (int offset = 0, i = 0; offset < buffer.Length; offset += 32, i++)
            {
                entries[i] = new ArraySegment<byte>(buffer, offset, 32);
            }

            return entries;
        }

        private ArraySegment<byte>[] CreateEmptyEntries(byte[] buffer)
        {
            ArraySegment<byte>[] entries = new ArraySegment<byte>[buffer.Length / 32];
            for (int offset = 0, i = 0; offset < buffer.Length; offset += 32, i++)
            {
                entries[i] = new ArraySegment<byte>(buffer, offset, 32);
            }

            return entries;
        }

        private byte GetChecksum(DirectoryEntry directoryEntry)
        {
            byte[] buffer = new byte[11];
            directoryEntry.Name.GetBytes(buffer, 0);
            byte checksum = FilenameChecksum(buffer, 0);
            return checksum;
        }

        private void EncodeEntry(ArraySegment<byte> entry, int ordinal, byte[] filename, byte shortNameChecksum, bool isLastEntry, bool isDeleted)
        {
            var buffer = entry.Array;
            var offset = entry.Offset;

            if (!isDeleted)
            {
                buffer[offset + LongDirectoryEntry.Offsets.Ordinal] = (byte)ordinal;
                if (isLastEntry)
                {
                    buffer[offset + LongDirectoryEntry.Offsets.Ordinal] |= LongDirectoryEntry.Masks.LastLongFileNameEntry;
                }
            }
            else
            {
                buffer[offset + LongDirectoryEntry.Offsets.Ordinal] = 0xE5;
            }

            buffer[offset + LongDirectoryEntry.Offsets.Flags] = (byte)(FatAttributes.ReadOnly | FatAttributes.Hidden | FatAttributes.System | FatAttributes.VolumeId);
            buffer[offset + LongDirectoryEntry.Offsets.Checksum] = shortNameChecksum;

            int nameIndex = (ordinal - 1) * 13 * 2; // 2 bytes per character
            System.Buffer.BlockCopy(filename, nameIndex + 0, buffer, offset + 1, 5 * 2);
            System.Buffer.BlockCopy(filename, nameIndex + 5 * 2, buffer, offset + 14, 6 * 2);
            System.Buffer.BlockCopy(filename, nameIndex + 5 * 2 + 6 * 2, buffer, offset + 28, 2 * 2);
        }
    }

    internal static class LongDirectoryEntry
    {
        /// <summary>
        /// Attributes set on a long vfat directory entry (ReadOnly, Hidden, System and VolumeId)
        /// </summary>
        public static FatAttributes Attributes = FatAttributes.ReadOnly | FatAttributes.Hidden |
                                                         FatAttributes.System | FatAttributes.VolumeId;
        public static class Masks
        {
            public const int LastLongFileNameEntry = 0x40;
            /// <summary>
            /// Mask to the lower 6 bits (0x3f).
            /// </summary>
            public const int LongFileNameEntryNumber = 0x3f;
        }

        public static FatAttributes GetAttributes(byte[] buffer)
        {
            return (FatAttributes)buffer[Offsets.Flags];
        }

        public static class Offsets
        {
            public const int Ordinal = 0x00;
            public const int Flags = 0x0b;
            public const int Checksum = 0x0d;
        }
    }

}
