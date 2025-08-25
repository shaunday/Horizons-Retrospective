using System;
using System.Collections.Generic;

namespace MW.Price.Fetcher.DataRanges
{
    public abstract class DataRange
    {
        public abstract DateTime Start { get; }
        public abstract DateTime End { get; }
        public abstract string Period { get; }
        public abstract string Bar { get; }

        protected const int MaxPointsPerRequest = 1000;

        public abstract IEnumerable<(DateTime from, DateTime to)> GetSubRanges();
    }

    public class FifteenYearsWeekly : DataRange
    {
        public override DateTime Start => DateTime.UtcNow.AddYears(-15);
        public override DateTime End => DateTime.UtcNow;
        public override string Period => "15y";
        public override string Bar => "1w";

        public override IEnumerable<(DateTime from, DateTime to)> GetSubRanges()
        {
            int totalWeeks = (int)Math.Ceiling((End - Start).TotalDays / 7);
            int chunks = (int)Math.Ceiling(totalWeeks / (double)MaxPointsPerRequest);
            int weeksPerChunk = (int)Math.Ceiling(totalWeeks / (double)chunks);

            var current = Start;
            for (int i = 0; i < chunks; i++)
            {
                var chunkEnd = current.AddDays(7 * weeksPerChunk);
                if (chunkEnd > End) chunkEnd = End;
                yield return (current, chunkEnd);
                current = chunkEnd;
            }
        }
    }

    public class FiveYearsDaily : DataRange
    {
        public override DateTime Start => DateTime.UtcNow.AddYears(-5);
        public override DateTime End => DateTime.UtcNow;
        public override string Period => "5y";
        public override string Bar => "1d";

        public override IEnumerable<(DateTime from, DateTime to)> GetSubRanges()
        {
            int totalDays = (int)(End - Start).TotalDays;
            int chunks = (int)Math.Ceiling(totalDays / (double)MaxPointsPerRequest);
            int daysPerChunk = (int)Math.Ceiling(totalDays / (double)chunks);

            var current = Start;
            for (int i = 0; i < chunks; i++)
            {
                var chunkEnd = current.AddDays(daysPerChunk);
                if (chunkEnd > End) chunkEnd = End;
                yield return (current, chunkEnd);
                current = chunkEnd;
            }
        }
    }

    public class OneYearFourHour : DataRange
    {
        public override DateTime Start => DateTime.UtcNow.AddYears(-1);
        public override DateTime End => DateTime.UtcNow;
        public override string Period => "1y";
        public override string Bar => "4h";

        public override IEnumerable<(DateTime from, DateTime to)> GetSubRanges()
        {
            int totalHours = (int)(End - Start).TotalHours;
            int bars = totalHours / 4;
            int chunks = (int)Math.Ceiling(bars / (double)MaxPointsPerRequest);
            int barsPerChunk = (int)Math.Ceiling(bars / (double)chunks);

            var current = Start;
            for (int i = 0; i < chunks; i++)
            {
                var chunkEnd = current.AddHours(4 * barsPerChunk);
                if (chunkEnd > End) chunkEnd = End;
                yield return (current, chunkEnd);
                current = chunkEnd;
            }
        }
    }

    public class ThreeMonthsOneHour : DataRange
    {
        public override DateTime Start => DateTime.UtcNow.AddMonths(-3);
        public override DateTime End => DateTime.UtcNow;
        public override string Period => "3m";
        public override string Bar => "1h";

        public override IEnumerable<(DateTime from, DateTime to)> GetSubRanges()
        {
            int totalHours = (int)(End - Start).TotalHours;
            int chunks = (int)Math.Ceiling(totalHours / (double)MaxPointsPerRequest);
            int hoursPerChunk = (int)Math.Ceiling(totalHours / (double)chunks);

            var current = Start;
            for (int i = 0; i < chunks; i++)
            {
                var chunkEnd = current.AddHours(hoursPerChunk);
                if (chunkEnd > End) chunkEnd = End;
                yield return (current, chunkEnd);
                current = chunkEnd;
            }
        }
    }
}
