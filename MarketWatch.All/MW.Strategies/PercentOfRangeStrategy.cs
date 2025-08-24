using System;
using System.Collections.Generic;
using System.Linq;

namespace MarketScanner.Strategies
{
    public class PercentOfRangeStrategy
    {
        public double ThresholdPercent { get; set; } // e.g., 10 = 10%
        public int LookbackDays { get; set; }

        public PercentOfRangeStrategy(double thresholdPercent, int lookbackDays)
        {
            ThresholdPercent = thresholdPercent;
            LookbackDays = lookbackDays;
        }

        // historicalPrices: oldest first
        public bool IsWithinThreshold(List<double> historicalPrices)
        {
            if (historicalPrices == null || historicalPrices.Count < LookbackDays)
                return false;

            var recent = historicalPrices.TakeLast(LookbackDays).ToList();
            double min = recent.Min();
            double max = recent.Max();
            double current = recent.Last();

            double percentOfRange = (current - min) / (max - min) * 100.0;
            return percentOfRange <= ThresholdPercent;
        }
    }
}
