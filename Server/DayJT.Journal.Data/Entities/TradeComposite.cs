using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DayJT.Journal.Data
{
    public class TradeComposite
    {
        [Key]
        public Guid Id { get; private set; } = Guid.NewGuid();

        public List<TradeElement> TradeElements { get; set; } = new List<TradeElement>();

        public DateTime CreatedAt { get; } = DateTime.Now;

    }
}
