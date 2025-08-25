using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketWatch.Entities
{
    public class PriceBarModel(DateTime time, decimal open, decimal high, decimal low, decimal close, long volume, Timeframe timeframe)
    {
        public int Id { get; set; } // Primary key for EF
        public int SecurityFK { get; set; }

        public DateTime Time { get; } = time;
        public decimal Open { get; } = open;
        public decimal High { get; } = high;
        public decimal Low { get; } = low;
        public decimal Close { get; } = close;
        public long Volume { get; } = volume;

        public Timeframe Timeframe { get; set; } = timeframe;
    }
}
