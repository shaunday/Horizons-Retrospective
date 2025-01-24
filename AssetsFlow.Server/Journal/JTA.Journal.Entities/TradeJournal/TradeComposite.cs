using HsR.Journal.Entities;
using System.ComponentModel.DataAnnotations;

public class TradeComposite
{
    [Key]
    public int Id { get; private set; }

    public ICollection<TradeElement> TradeElements { get; set; } = new List<TradeElement>();

    public ICollection<string> Sectors { get; set; } = new List<string>();

    public TradeElement? Summary { get; set; }

    public TradeStatus Status { get; set; } = TradeStatus.AnIdea;

    public DateTime? OpenedAt { get; set; }

    public DateTime? ClosedAt { get; set; }

    public override string ToString()
    {
        return $"Id={Id}, Status={Status}, Trade Eles Count={TradeElements.Count}";
    }
}
