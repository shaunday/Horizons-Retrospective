using HsR.Journal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HsR.Journal.Services
{
    public abstract class FilterDefinition
    {
        public FilterId Id { get; init; }
        public required string Title { get; init; }
        public FilterType Type { get; init; }
    }

    public class EnumFilterDefinition<TEnum> : FilterDefinition where TEnum : struct, Enum
    {
        public required IReadOnlyList<TEnum> Restrictions { get; init; }
    }

    public class TextFilterDefinition : FilterDefinition
    {
        public IReadOnlyList<string> Restrictions { get; init; } = Array.Empty<string>();
    }

    public class DateRangeFilterDefinition : FilterDefinition
    {
        public IReadOnlyList<string> Restrictions { get; init; } = Array.Empty<string>();
    }

    public enum FilterType
    {
        Enum,
        Text,
        DateRange
    }

    public enum WinLoss
    {
        Win,
        Loss
    }

}
