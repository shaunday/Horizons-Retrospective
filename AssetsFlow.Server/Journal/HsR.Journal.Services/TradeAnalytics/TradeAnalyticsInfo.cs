namespace HsR.Journal.TradeAnalytics
{
    public class TradeAnalyticsInfo
    {
        public double TotalCost { get; set; }
        public double TotalAmount { get; set; }
        public double AveragePrice => TotalAmount > 0 ? TotalCost / TotalAmount : 0;
    }
}
