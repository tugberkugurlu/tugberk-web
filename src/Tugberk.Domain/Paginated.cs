using System;
using System.Collections.Generic;

namespace Tugberk.Domain
{
    public class Paginated<T>
    {
        public Paginated(IReadOnlyCollection<T> items, int skipped, int totalCount)
        {
            Items = items ?? throw new ArgumentNullException(nameof(items));

            if (skipped < 0) throw new ArgumentOutOfRangeException(nameof(skipped));
            if (totalCount < 0) throw new ArgumentOutOfRangeException(nameof(totalCount));

            Skipped = skipped;
            TotalCount = totalCount;
        }

        public IReadOnlyCollection<T> Items { get; }
        public int Skipped { get; }
        public int TotalCount { get; }

        public bool HasPrevious => Skipped > 0;
        public bool HasNext => TotalCount > Skipped + Items.Count;
    }
}