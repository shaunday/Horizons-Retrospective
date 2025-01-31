using HsR.Ticker.Entity;
using System.ComponentModel.DataAnnotations;

namespace HsR.PriceAlerts.Entities
{
    public class TickerWithPriceAlerts : TickerInfo
    {
        public List<PriceAlertInfo> PriceAlerts { get; set; } = [];

        public DateTime LastNotificationTimeStamp { get; set; }
    }
}
