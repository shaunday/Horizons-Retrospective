using AssetsFlowWeb.Services.Models;
using HsR.Journal.Entities;
using HsR.Journal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetsFlowWeb.Services.Filtering
{
    public static class FilterDefinitionFactory
    {
        public static ICollection<FilterDefinition> Create()
        {
            return new List<FilterDefinition>
        {
            new EnumFilterDefinition<WinLoss>
            {
                Id = FilterId.Wl,
                Title = "W/L",
                Type = FilterType.Enum,
            },
            new EnumFilterDefinition<TradeStatus>
            {
                Id = FilterId.Status,
                Title = "Status",
                Type = FilterType.Enum,
            },
            new TextFilterDefinition
            {
                Id = FilterId.Symbol,
                Title = "Symbol",
                Type = FilterType.Text
            },
            //new DateRangeFilterDefinition
            //{
            //    Id = FilterId.OpenDateRange,
            //    Title = "Open Date",
            //    Type = FilterType.DateRange
            //},
            //new DateRangeFilterDefinition
            //{
            //    Id = FilterId.CloseDateRange,
            //    Title = "Close Date",
            //    Type = FilterType.DateRange
            //}
        };
        }
    }

}
