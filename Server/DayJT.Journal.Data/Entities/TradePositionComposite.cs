using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DayJT.Journal.Data
{
    public class TradePositionComposite
    {
        [Key]
        public Guid Id { get; private set; } = Guid.NewGuid();

        public ICollection<TradeComponent> TradeComponents { get; set; } = new List<TradeComponent>();

        public DateTime CreatedAt { get; } = DateTime.Now;

    }
}
