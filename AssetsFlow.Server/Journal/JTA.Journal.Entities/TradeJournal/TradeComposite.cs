using HsR.Journal.Entities;
using HsR.Journal.Entities.TradeJournal;
using System.ComponentModel.DataAnnotations;

public class TradeComposite
{
    [Key]
    public int Id { get; private set; }

    public ICollection<InterimTradeElement> TradeElements { get; set; } = new List<InterimTradeElement>();

    public ICollection<string> Sectors { get; set; } = new List<string>();

    public TradeSummary? Summary { get; set; }

    public TradeStatus Status { get; set; } = TradeStatus.AnIdea;

    public void Activate()
    {
        Status = TradeStatus.Open;
    }

    public DateTime? OpenedAt { get; set; }

    public DateTime? ClosedAt { get; set; }

    public override string ToString() => $"Id={Id}, Status={Status}, Trade Eles Count={TradeElements.Count}";
}
