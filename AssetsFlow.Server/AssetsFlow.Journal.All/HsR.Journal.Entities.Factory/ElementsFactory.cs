using HsR.Common.Extenders;
using HsR.Journal.Entities.TradeJournal;
using HsR.Journal.TradeAnalytics;
using System;
using System.Collections.Generic;

namespace HsR.Journal.Entities.Factory
{
    public static partial class TradeElementsFactory
    {
        private static readonly Dictionary<TradeActionType, Func<TradeComposite, TradeElement>> FactoryMethods =
            new()
            {
                { TradeActionType.Origin, trade => GetNewOrigin(trade) },
                { TradeActionType.Add, trade => GetNewInterimTradeElement(trade, true) },
                { TradeActionType.Reduce, trade => GetNewInterimTradeElement(trade, false) },
                { TradeActionType.Evaluation, trade => GetNewEvalutationElement(trade) },
                { TradeActionType.Summary, trade => GetNewSummary(trade) },
                { TradeActionType.Closure, trade => GetNewSummary(trade, isClosure: true) }
            };

        public static TradeElement GetNewElement(TradeComposite trade, TradeActionType actionType)
        {
            if (FactoryMethods.TryGetValue(actionType, out var factoryMethod))
            {
                var newTradeElement = factoryMethod(trade);
                newTradeElement.UserId = trade.UserId;

                // Set UserId for all entries
                foreach (var entry in newTradeElement.Entries)
                {
                    entry.UserId = trade.UserId;
                }

                return newTradeElement;
            }
            throw new ArgumentException($"Unsupported TradeActionType: {actionType}");
        }

        private static InterimTradeElement GetNewOrigin(TradeComposite trade)
        {
            InterimTradeElement originElement = new(trade, TradeActionType.Origin);
            originElement.Entries = GetOriginEntries(originElement);
            return originElement;
        }

        private static InterimTradeElement GetNewInterimTradeElement(TradeComposite trade, bool isAdd)
        {
            InterimTradeElement tradeInput = new(trade, isAdd ? TradeActionType.Add : TradeActionType.Reduce);
            tradeInput.Entries = isAdd? GetAddPositionEntries(tradeInput) : GetReducePositionEntries(tradeInput);
            return tradeInput;
        }

        private static TradeSummary GetNewSummary(TradeComposite trade, bool isClosure = false)
        {
            var analytics = Analytics.GetTradingCosts(trade);
            TradeAnalyticsSummary analyticsSummary = new(analytics);

            TradeSummary newSummary = new(trade, TradeActionType.Summary);
            newSummary.IsInterim = analyticsSummary.IsNetExists;
            newSummary.Entries = analyticsSummary.IsNetExists && !isClosure
                ? GetSummaryComponents(newSummary, analyticsSummary)
                : GetTradeClosureComponents(newSummary, analyticsSummary);

            return newSummary;
        }

        private static InterimTradeElement GetNewEvalutationElement(TradeComposite trade)
        {
            InterimTradeElement tradeOverview = new(trade, TradeActionType.Evaluation);
            tradeOverview.Entries = GetEvalutationEntries(tradeOverview);
            return tradeOverview;
        }

    }
}
