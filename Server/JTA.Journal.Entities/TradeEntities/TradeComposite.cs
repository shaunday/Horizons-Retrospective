using System.ComponentModel.DataAnnotations;

namespace DayJT.Journal.DataEntities.Entities
{
    public class TradeComposite
    {
        [Key]
        public int Id { get; private set; }

        public ICollection<TradeElement> TradeElements { get; set; } = new List<TradeElement>(); //only called when I new this manually, not called on DB access.

        public List<string> Sectors { get; set; } = [];

        public TradeElement Summary { get; set; } = null!;

        [Required]
        public DateTime CreatedAt { get; } = DateTime.Now;

    }
}
