using HsR.Journal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HsR.Journal.DataContext
{
    public static class TradesFiltering
    {
        public static IQueryable<TradeComposite> ApplyFiltering(this IQueryable<TradeComposite> query, TradesFilterModel filter)
        {
            if (filter.OpenLowerLimit.HasValue)
            {
                query = query.Where(t => t.OpenedAt >= filter.OpenLowerLimit.Value);
            }

            if (filter.OpenUpperLimit.HasValue)
            {
                query = query.Where(t => t.OpenedAt <= filter.OpenUpperLimit.Value);
            }

            if (filter.CloseLowerLimit.HasValue)
            {
                query = query.Where(t => t.OpenedAt >= filter.CloseLowerLimit.Value);
            }

            if (filter.CloseUpperLimit.HasValue)
            {
                query = query.Where(t => t.OpenedAt <= filter.CloseUpperLimit.Value);
            }

            if (filter.FilterObjects != null && filter.FilterObjects.Count != 0)
            {
                foreach (var filterObject in filter.FilterObjects)
                {
                    query = query.Where(t => t.TradeElements.Any(te => te.Entries.Any(e =>
                                    e.Title == filterObject.Title &&
                                     (e.Content == null && string.IsNullOrEmpty(filterObject.FilterValue) ||
                                      e.Content != null && e.Content.Contains(filterObject.FilterValue)))));
                }
            }

            return query;
        }
    }
}
