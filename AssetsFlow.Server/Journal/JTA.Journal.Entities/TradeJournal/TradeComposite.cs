using HsR.Journal.Entities;
using HsR.Journal.Entities.TradeJournal;
using System.ComponentModel.DataAnnotations;

public class TradeComposite
{
    [Key]
    public int Id { get; private set; }

    public ICollection<InterimTradeElement> TradeElements { get; set; } = new List<InterimTradeElement>();

    public TradeSummary? Summary { get; set; }

    #region Status Logic

    public bool IsPending => TradeElements.Any(ele => !ele.AllowActivation());
    public TradeStatus Status { get; set; } = TradeStatus.AnIdea;

    private void UpdateStatus(TradeStatus newStatus)
    {
        Status = newStatus;

        if (newStatus == TradeStatus.Open)
        {
            OpenedAt ??= DateTime.UtcNow;
        }
        else if (newStatus == TradeStatus.Closed)
        {
            ClosedAt = DateTime.UtcNow;
        }
    }

    public void Activate()
    {
        if (Status == TradeStatus.AnIdea)
        {
            UpdateStatus(TradeStatus.Open);
        }
    }

    public void Close()
    {
        UpdateStatus(TradeStatus.Closed);
    } 
    #endregion

    public DateTime? OpenedAt { get; set; }

    public DateTime? ClosedAt { get; set; }

    public override string ToString() => $"Id={Id}, Status={Status}, Trade Eles Count={TradeElements.Count}";
}
