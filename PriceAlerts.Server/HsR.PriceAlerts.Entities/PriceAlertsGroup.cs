using HsR.Ticker.Entity;
using System.ComponentModel.DataAnnotations;

namespace HsR.PriceAlerts.Entities
{
    public class PriceAlertsGroup
    {
        [Key]
        public int Id { get; private set; }

        [Required]
        public TickerInfo Ticker { get; set; } = null!;

        public List<PriceAlertInfo> PriceAlerts { get; set; } = [];
    }
}
