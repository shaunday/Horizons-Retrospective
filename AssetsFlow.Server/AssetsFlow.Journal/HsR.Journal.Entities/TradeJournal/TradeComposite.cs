using HsR.Journal.Entities;
using HsR.Journal.Entities.TradeJournal;
using System.ComponentModel.DataAnnotations;

namespace HsR.Journal.Entities
{
    public class TradeComposite
    {
        [Key]
        public int Id { get; private set; }

        [Required]
        public Guid UserId { get; set; }

        public ICollection<InterimTradeElement> TradeElements { get; set; } = [];

        public TradeSummary? Summary { get; set; }

        #region Status Logic

        public TradeStatus Status { get; private set; } = TradeStatus.AnIdea;

        public void SetStatus(TradeStatus newStatus)
        {
            if (Status == newStatus)
                return;

            Status = newStatus;
        }

        #endregion

        public override string ToString() => $"Id={Id}, Status={Status}, Trade Eles Count={TradeElements.Count}";
    }
}