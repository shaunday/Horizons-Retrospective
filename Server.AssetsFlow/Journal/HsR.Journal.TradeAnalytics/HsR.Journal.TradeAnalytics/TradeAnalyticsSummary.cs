namespace HsR.Journal.TradeAnalytics
{
    public struct TradeAnalyticsSummary
    {
        public TradeAnalyticsInfo AddTrades { get; }
        public TradeAnalyticsInfo ReduceTrades { get; }

        public double AverageEntryPrice => AddTrades.AveragePrice;
        public double AverageExitPrice => ReduceTrades.AveragePrice;

        public double Profit => AddTrades.TotalCost - ReduceTrades.TotalCost;


        public double NetAmount => AddTrades.TotalAmount - ReduceTrades.TotalAmount;
        public double NetCost => AddTrades.TotalCost - ReduceTrades.TotalCost;

        public TradeAnalyticsSummary((TradeAnalyticsInfo addTrades, TradeAnalyticsInfo reduceTrades) trades)
        {
            AddTrades = trades.addTrades;
            ReduceTrades = trades.reduceTrades;
        }
    }
}
