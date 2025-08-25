using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketWatch.Entities
{
    /// <summary>
    /// Supported bar timeframes.
    /// </summary>
    public enum Timeframe
    {
        Weekly,
        Daily,
        FourHour,
        OneHour,
        OneMinute
    }

    /// <summary>
    /// Type of security.
    /// </summary>
    public enum SecurityType
    {
        Stock,
        Etf,
        Crypto,
        Option,
        Future,
        Bond,
        Cash
    }
}
