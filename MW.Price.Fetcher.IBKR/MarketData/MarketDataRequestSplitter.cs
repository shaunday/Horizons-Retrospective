using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MW.Price.Fetcher.IBKR.MarketData
{
    public static class MarketDataRequestSplitter
    {
        public static IEnumerable<(DateTime from, DateTime to)> SplitByMaxPoints(
            DateTime start, DateTime end, TimeSpan barSize, int maxPoints)
        {
            int barsPerRequest = maxPoints;
            TimeSpan totalDuration = TimeSpan.FromTicks(barSize.Ticks * barsPerRequest);

            DateTime current = start;
            while (current < end)
            {
                DateTime next = current.Add(totalDuration);
                if (next > end) next = end;
                yield return (current, next);
                current = next;
            }
        }
    }

}
