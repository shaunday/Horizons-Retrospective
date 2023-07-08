using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DayJT.Journal.Data
{
    public class TradePositionComposite
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public ICollection<TradeComponent> TradeComponents { get; set; } = new List<TradeComponent>();

        public DateTime CreatedAt { get; } = DateTime.Now;

    }
}
