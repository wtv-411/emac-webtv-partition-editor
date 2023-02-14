using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace webtv_partition_editor
{
    /// <summary>
    /// P/Invoke wrappers around Win32 functions and constants.
    /// </summary>
    public partial class WebTVDiskIO
    {
        #region Constants used in unmanaged functions
        public const uint FILE_ANY_ACCESS = 0x00000001;
        public const uint FILE_READ_ACCESS = 0x00000001;
        public const uint FILE_WRITE_ACCESS = 0x00000002;
        public const uint FILE_SHARE_READ = 0x00000001;
        public const uint FILE_SHARE_WRITE = 0x00000002;
        public const uint FILE_SHARE_DELETE = 0x00000004;
        public const uint OPEN_EXISTING = 3;
        public const uint FILE_DEVICE_CONTROLLER = 0x00000004;
        public const uint IOCTL_SCSI_BASE = FILE_DEVICE_CONTROLLER;
        public const uint METHOD_BUFFERED = 0;

        public const uint GENERIC_READ = (0x80000000);
        public const uint GENERIC_WRITE = (0x40000000);

        public const uint FILE_FLAG_NO_BUFFERING = 0x20000000;
        public const uint FILE_FLAG_WRITE_THROUGH = 0x80000000;
        public const uint FILE_READ_ATTRIBUTES = (0x0080);
        public const uint FILE_WRITE_ATTRIBUTES = 0x0100;
        public const uint ERROR_INSUFFICIENT_BUFFER = 122;

        public const int FSCTL_LOCK_VOLUME = 0x00090018;
        public const int FSCTL_UNLOCK_VOLUME = 0x0009001c;
        public const int FSCTL_DISMOUNT_VOLUME = 0x00090020;
        public const int IOCTL_STORAGE_EJECT_MEDIA = 0x2D4808;
        public const int IOCTL_STORAGE_MEDIA_REMOVAL = 0x002D4804;

        public const uint FILE_DEVICE_MASS_STORAGE = 0x0000002d;
        public const uint IOCTL_STORAGE_BASE = FILE_DEVICE_MASS_STORAGE;

        public const uint ATA_FLAGS_DRDY_REQUIRED = 0x01;
        public const uint ATA_FLAGS_DATA_IN = 0x02;

        public const byte ATA_IDENTIFY_DEVICE = 0xEC;

        public const byte ATA_SECURITY_SET_PASSWORD = 0xF1;
        public const byte ATA_SECURITY_UNLOCK = 0xF2;
        public const byte ATA_SECURITY_ERASE_PREPARE = 0xF3;
        public const byte ATA_SECURITY_ERASE_UNIT = 0xF4;
        public const byte ATA_SECURITY_FREEZE = 0xF5;
        public const byte ATA_SECURITY_DISABLE_PASSWORD = 0xF6;

        public const int ATA_BLOCK_SIZE = 512;

        // https://wiki.osdev.org/ATA_PIO_Mode#Error_Register
        public const int ATA_REGISTER_ERROR = 0;
        /*
           Bit Abbreviation    Function
           0	AMNF            Address mark not found.
           1	TKZNF           Track zero not found.
           2	ABRT            Aborted command.
           3	MCR             Media change request.
           4	IDNF            ID not found.
           5	MC              Media changed.
           6	UNC             Uncorrectable data error.
           7	BBK             Bad Block detected.
        */
        public const int ATA_REGISTER_FEATURES = 0;
        public const int ATA_REGISTER_SECTOR_COUNT = 1;
        public const int ATA_REGISTER_SECTOR_NUMBER = 2;
        public const int ATA_REGISTER_CYLINDER_LOW = 3;
        public const int ATA_REGISTER_CYLINDER_HIGH = 4;
        public const int ATA_REGISTER_DEVICE_HEAD = 5;
        public const int ATA_REGISTER_STATUS = 6;
        public const int ATA_REGISTER_COMMAND = 6;
        public const int ATA_REGISTER_RESERVED = 7;
        #endregion


        #region Structs used in unmanaged functions
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct ATA_PASS_THROUGH_EX
        {
            public ushort length;
            public ushort ataFlags;
            public byte pathId;
            public byte targetId;
            public byte lun;
            public byte reservedAsUchar;
            public uint dataTransferLength;
            public uint timeOutValue;
            public uint reservedAsUlong;
            public IntPtr dataBufferOffset;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] previousTaskFile;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] currentTaskFile;
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct GENERIC_ATA_QUERY
        {
            public ATA_PASS_THROUGH_EX header;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public ushort[] data;
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct GENERIC_ATA_RESULT
        {
            public ATA_PASS_THROUGH_EX header;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public ushort[] data;
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct SECURITY_UNLOCK_ATA_QUERY
        {
            public ATA_PASS_THROUGH_EX header;
            public ushort identifier;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] password;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 239)]
            public ushort[] reserved;
        }
        #endregion

        #region Unamanged function declarations

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(SafeFileHandle hHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static unsafe extern SafeFileHandle CreateFile(
            string FileName,
            uint DesiredAccess,
            uint ShareMode,
            IntPtr SecurityAttributes,
            uint CreationDisposition,
            uint FlagsAndAttributes,
            IntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool DeviceIoControl(
            SafeFileHandle hDevice,
            uint dwIoControlCode,
            ref GENERIC_ATA_QUERY lpInBuffer,
            uint nInBufferSize,
            ref GENERIC_ATA_RESULT lpOutBuffer,
            uint nOutBufferSize,
            out uint lpBytesReturned,
            IntPtr lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool DeviceIoControl(
            SafeFileHandle hDevice,
            uint dwIoControlCode,
            ref SECURITY_UNLOCK_ATA_QUERY lpInBuffer,
            uint nInBufferSize,
            ref GENERIC_ATA_RESULT lpOutBuffer,
            uint nOutBufferSize,
            out uint lpBytesReturned,
            IntPtr lpOverlapped);

        [DllImport("kernel32.dll")]
        public static extern bool FlushFileBuffers(
            SafeFileHandle hFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern unsafe bool ReadFile(
            SafeFileHandle hFile,
            byte* pBuffer,
            uint NumberOfBytesToRead,
            uint* pNumberOfBytesRead,
            IntPtr Overlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetFilePointerEx(
            SafeFileHandle hFile,
            ulong liDistanceToMove,
            out ulong lpNewFilePointer,
            uint dwMoveMethod);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern unsafe bool WriteFile(
            SafeFileHandle hFile,
            byte* pBuffer,
            uint NumberOfBytesToWrite,
            uint* pNumberOfBytesWritten,
            IntPtr Overlapped);
        #endregion
    }
}
