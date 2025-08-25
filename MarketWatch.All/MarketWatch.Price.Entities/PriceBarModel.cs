using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketWatch.Entities
{
    public class PriceBarModel
    {
        public int Id { get; set; } // Primary key for EF
        public int SecurityFK { get; set; }

        public DateTime Time { get; }
        public decimal Open { get; }
        public decimal High { get; }
        public decimal Low { get; }
        public decimal Close { get; }
        public long Volume { get; }

        public PriceBarModel(DateTime time, decimal open, decimal high, decimal low, decimal close, long volume)
        {
            Time = time;
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;
        }
    }
}
