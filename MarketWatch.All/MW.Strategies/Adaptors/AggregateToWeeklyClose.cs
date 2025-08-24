using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MW.Strategies.Adaptors
{
    public static class Adaptors
    {
        public static List<double> AggregateToWeeklyClose(List<double> dailyPrices)
        {
            var weekly = new List<double>();
            for (int i = 0; i < dailyPrices.Count; i += 5)
            {
                int end = Math.Min(i + 5, dailyPrices.Count);
                weekly.Add(dailyPrices[end - 1]);
            }
            return weekly;
        }
    }
}
