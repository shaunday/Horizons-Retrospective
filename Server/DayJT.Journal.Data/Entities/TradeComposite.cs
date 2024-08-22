using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DayJT.Journal.Data
{
    public class TradeComposite
    {
        [Key]
        public int Id { get; private set; }

        public List<TradeElement> TradeElements { get; set; } = new List<TradeElement>();

        public TradeElement Summary { get; set; } = null!;

        public DateTime CreatedAt { get; } = DateTime.Now;

    }
}
