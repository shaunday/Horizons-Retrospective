using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MW.Strategies.Adaptors;

namespace MW.Strategies
{
    public enum BBPosition
    {
        Top,
        Bottom
    }


    public class BollingerBandStrategy
    {
        public double ThresholdPercent { get; set; } // e.g., 2 = within 2% of BB edge
        public int LookbackDays { get; set; }
        public BBPosition Position { get; set; }

        public BollingerBandStrategy(double thresholdPercent, int lookbackDays, BBPosition position)
        {
            ThresholdPercent = thresholdPercent;
            LookbackDays = lookbackDays;
            Position = position;
        }

        // historicalPrices: oldest first
        public bool IsNearBand(List<double> dailyPrices)
        {
            if (dailyPrices == null || dailyPrices.Count < LookbackDays)
                return false;

            var recent = dailyPrices.TakeLast(LookbackDays).ToList();
            var weeklyCloses = Adaptors.AggregateToWeeklyClose(recent);

            double avg = weeklyCloses.Average();
            double std = Math.Sqrt(weeklyCloses.Sum(p => Math.Pow(p - avg, 2)) / weeklyCloses.Count);

            double bandValue = Position == BBPosition.Bottom ? avg - 2 * std : avg + 2 * std;
            double current = weeklyCloses.Last();

            double distancePercent = ((current - bandValue) / bandValue) * 100.0;
            return Math.Abs(distancePercent) <= ThresholdPercent;
        } 
    }
}
