using HsR.Journal.Entities;
using HsR.Journal.Entities.TradeJournal;
using System.ComponentModel.DataAnnotations;

public class TradeComposite
{
    [Key]
    public int Id { get; private set; }

    public ICollection<TradeAction> TradeElements { get; set; } = new List<TradeAction>();

    public ICollection<string> Sectors { get; set; } = new List<string>();

    public TradeSummary? Summary { get; set; }

    public TradeStatus Status { get; set; } = TradeStatus.AnIdea;

    public DateTime? OpenedAt { get; set; }

    public DateTime? ClosedAt { get; set; }

    public override string ToString() => $"Id={Id}, Status={Status}, Trade Eles Count={TradeElements.Count}";
}
