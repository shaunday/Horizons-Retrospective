using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DayJT.Journal.Data
{
    public class TradeComposite
    {
        [Key]
        public int Id { get; private set; }

        public List<TradeElement> TradeElements { get; set; } = new List<TradeElement>(); //only called when I new this manually, not called on DB access.

        public string Sector { get; set; } = "";

        public TradeElement Summary { get; set; } = null!;

        public DateTime CreatedAt { get; } = DateTime.Now;

    }
}
