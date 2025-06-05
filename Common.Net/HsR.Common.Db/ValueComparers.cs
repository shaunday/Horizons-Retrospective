using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Linq;

public static class ValueComparers
{
    public static readonly ValueComparer<ICollection<string>?> StringCollectionComparer =
    new ValueComparer<ICollection<string>?>(
        (c1, c2) =>
            ReferenceEquals(c1, c2) ||
            (c1 != null && c2 != null && c1.SequenceEqual(c2)),
        c => c == null ? 0 : c.Aggregate(0, (a, v) => HashCode.Combine(a, v != null ? v.GetHashCode() : 0)),
        c => c == null ? null : c.ToList()
    );
}
