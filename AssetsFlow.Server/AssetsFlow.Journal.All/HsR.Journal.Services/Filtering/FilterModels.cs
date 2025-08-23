using HsR.Journal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HsR.Journal.Services
{
    public abstract class FilterDefinition
    {
        public FilterId Id { get; init; }
        public required string Title { get; init; }
        public FilterType Type { get; init; }

        // Single Restrictions property in the base
        public List<string> Restrictions { get; init; } = [];
    }

    public class EnumFilterDefinition<TEnum> : FilterDefinition where TEnum : struct, Enum
    {
        public EnumFilterDefinition()
        {
            // Automatically set Restrictions to string representations of the enum
            Restrictions = Enum.GetValues<TEnum>().Select(e => e.ToString()).ToList();
        }
    }

    public class TextFilterDefinition : FilterDefinition { }

    public class DateRangeFilterDefinition : FilterDefinition { }

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
