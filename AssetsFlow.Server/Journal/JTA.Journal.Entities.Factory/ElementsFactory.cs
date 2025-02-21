using HsR.Common.Extenders;
using HsR.Journal.Entities.TradeJournal;
using HsR.Journal.TradeAnalytics;
using System;
using System.Collections.Generic;

namespace HsR.Journal.Entities.Factory
{
    public static class TradeElementsFactory
    {
        private static readonly Dictionary<TradeActionType, Func<TradeComposite, TradeElement>> FactoryMethods =
            new()
            {
                { TradeActionType.Origin, trade => GetNewOrigin(trade) },
                { TradeActionType.Add, trade => GetNewInterimTradeElement(trade, true) },
                { TradeActionType.Reduce, trade => GetNewInterimTradeElement(trade, false) },
                { TradeActionType.Evaluation, trade => GetNewEvalutationElement(trade) },
                { TradeActionType.Summary, trade => GetNewSummary(trade) }
            };

        public static TradeElement GetNewElement(TradeComposite trade, TradeActionType actionType)
        {
            if (FactoryMethods.TryGetValue(actionType, out var factoryMethod))
            {
                return (factoryMethod(trade)); // Adjust additionalData if needed
            }
            throw new ArgumentException($"Unsupported TradeActionType: {actionType}");
        }

        private static InterimTradeElement GetNewOrigin(TradeComposite trade)
        {
            InterimTradeElement originElement = new(trade, TradeActionType.Origin);
            originElement.Entries = EntriesFactory.GetOriginEntries(originElement);
            return originElement;
        }

        private static InterimTradeElement GetNewInterimTradeElement(TradeComposite trade, bool isAdd)
        {
            InterimTradeElement tradeInput = new(trade, isAdd ? TradeActionType.Add : TradeActionType.Reduce);
            tradeInput.Entries = isAdd
                ? (trade.Status == TradeStatus.AnIdea
                    ? EntriesFactory.GetFirstPositionEntries(tradeInput)
                    : EntriesFactory.GetAddPositionEntries(tradeInput))
                : EntriesFactory.GetReducePositionEntries(tradeInput);
            return tradeInput;
        }

        public static TradeSummary GetNewSummary(TradeComposite trade)
        {
            var analytics = Analytics.GetTradingCosts(trade);
            TradeAnalyticsSummary analyticsSummary = new(analytics);

            TradeSummary newSummary = new(trade, TradeActionType.Summary);
            newSummary.IsInterim = analyticsSummary.IsNetExists;
            newSummary.Entries = analyticsSummary.IsNetExists
                ? EntriesFactory.GetTradeClosureComponents(newSummary, analyticsSummary)
                : EntriesFactory.GetSummaryComponents(newSummary, analyticsSummary);

            return newSummary;
        }

        private static InterimTradeElement GetNewEvalutationElement(TradeComposite trade)
        {
            InterimTradeElement tradeOverview = new(trade, TradeActionType.Evaluation);
            tradeOverview.Entries = EntriesFactory.GetEvalutationEntries(tradeOverview);
            return tradeOverview;
        }
    }
}
