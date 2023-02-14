namespace DiscUtils.Fat
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a list of free directory entries. Provides methods to add or retrieve one or more directory entries.
    /// </summary>
    internal class FreeList
    {
        private List<long> _freeEntries;
        /// <summary>
        /// The size of each entry
        /// </summary>
        private int _entrySize;

        public FreeList(int entrySize)
        {
            _freeEntries = new List<long>();
            _entrySize = entrySize;
        }
        public int Count
        {
            get { return _freeEntries.Count; }
        }

        public int EntrySize
        {
            get { return _entrySize; }
        }

        public void Add(long value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException("value", "Parameter cannot be less than zero.");
            }

            if ((value % _entrySize) != 0)
            {
                throw new ArgumentOutOfRangeException("value", "Parameter is not a multiple of the entry size.");
            }

            _freeEntries.Add(value);
        }

        /// <summary>
        /// Gets a free entry.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>
        /// <c>true</c> if the <see cref="FreeList"/> was able return a free entry; otherwise <c>false</c>
        /// </returns>
        public bool TryGet(out long value)
        {
            if (_freeEntries.Count > 0)
            {
                value = _freeEntries[0];
                _freeEntries.RemoveAt(0);
                return true;
            }

            value = -1;
            return false;
        }

        /// <summary>
        /// Get <paramref name="count"/> consecutive free entries.
        /// </summary>
        /// <param name="count"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGet(int count, out long value)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException("count", "Parameter 'count' must be greater than zero");
            }

            if (count == 1)
            {
                return TryGet(out value);
            }

            // Unlink an entry from the free list (or add to the end of the existing directory)
            if (_freeEntries.Count >= count)
            {
                // we need to find 'count' entries that are side by side
                var indexes = FindConsecutiveItems(_freeEntries, count);
                if (indexes.Count != 0)
                {
                    indexes.Sort();

                    value = Int64.MaxValue;
                    for (int i = indexes.Count - 1; 0 <= i; i--)
                    {
                        value = Math.Min(value, _freeEntries[indexes[i]]);
                        _freeEntries.RemoveAt(indexes[i]);
                    }

                    return true;
                }
            }

            value = -1;
            return false;
        }

        private List<int> FindConsecutiveItems(List<long> freeEntries, int requiredCount)
        {
            var ordered = CreateSortedList(freeEntries);

            List<int> indexes = new List<int>();

            int foundCount = 0;
            long firstItem = 0; // Irrelevant to start with

            for (int i = 0; i < ordered.Keys.Count; i++)
            {
                long position = ordered.Keys[i];

                // First value in the ordered list: start of a sequence
                if (foundCount == 0)
                {
                    firstItem = position;
                    foundCount = 1;
                    indexes.Add(ordered[position]);
                }
                // New value contributes to sequence
                else if (position == firstItem + foundCount * _entrySize)
                {
                    indexes.Add(ordered[position]);

                    // found 'requiredCount' entries side by side
                    if (++foundCount == requiredCount)
                    {
                        return indexes;
                    }
                }
                else
                {
                    // start again
                    firstItem = position;
                    foundCount = 1;
                    indexes.Clear();
                    indexes.Add(ordered[position]);
                }
            }

            return new List<int>();
        }

        private SortedList<long, int> CreateSortedList(List<long> freeEntries)
        {
            SortedList<long, int> ordered = new SortedList<long, int>();
            for (int index = 0; index < _freeEntries.Count; index++)
            {
                ordered.Add(freeEntries[index], index);
            }
            return ordered;
        }
    }
}
