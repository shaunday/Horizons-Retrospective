using System.ComponentModel.DataAnnotations;

namespace HsR.Journal.Entities
{
    public class TradeComposite
    {
        [Key]
        public int Id { get; private set; }

        public ICollection<TradeElement> TradeElements { get; set; } = []; //only called when I new this manually, not called on DB access.

        public ICollection<string> Sectors { get; set; } = [];

        public TradeElement? Summary { get; set; }

        public TradeStatus Status { get; set; } = TradeStatus.AnIdea;

        public DateTime? OpenedAt { get; set; }

        public DateTime? ClosedAt { get; set; }

    }
}
