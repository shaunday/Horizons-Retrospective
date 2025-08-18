using HsR.Journal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HsR.Journal.Repository.Services.TradeCompositeRepo
{
    public static class TradesFiltering
    {
        public static IQueryable<TradeComposite> ApplyFiltering(this IQueryable<TradeComposite> query, TradesFilterModel filter)
        {
            //if (filter.OpenLowerLimit.HasValue)
            //{
            //    query = query.Where(t => t.OpenedAt >= filter.OpenLowerLimit.Value);
            //}

            //if (filter.OpenUpperLimit.HasValue)
            //{
            //    query = query.Where(t => t.OpenedAt <= filter.OpenUpperLimit.Value);
            //}

            //if (filter.CloseLowerLimit.HasValue)
            //{
            //    query = query.Where(t => t.ClosedAt >= filter.CloseLowerLimit.Value);
            //}

            //if (filter.CloseUpperLimit.HasValue)
            //{
            //    query = query.Where(t => t.ClosedAt <= filter.CloseUpperLimit.Value); 
            //}

            if (filter.FilterObjects?.Any() == true)
            {
                query = query.Where(t =>
                    t.TradeElements.Any(te =>
                        te.Entries.Any(e => filter.FilterObjects.Any(f =>
                            e.Title == f.Title &&
                            (string.IsNullOrEmpty(f.FilterValue) || (e.Content != null && e.Content.Contains(f.FilterValue)))
                        ))
                    )
                );
            }

            return query;
        }
    }
}
