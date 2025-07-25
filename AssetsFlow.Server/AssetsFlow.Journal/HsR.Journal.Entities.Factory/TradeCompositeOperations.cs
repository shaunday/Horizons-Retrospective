﻿using HsR.Common.Extenders;
using HsR.Journal.Entities.TradeJournal;
using HsR.Journal.TradeAnalytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HsR.Journal.Entities.Factory
{
    public class TradeCompositeOperations
    {
        public static TradeComposite CloseTrade(TradeComposite trade, string closingPrice)
        {
            // Create a TradeElement for ReducePosition
            InterimTradeElement? reductionEle = TradeElementsFactory.GetNewElement(trade, TradeActionType.Reduce) as InterimTradeElement;

            if (reductionEle == null)
            {
                throw new InvalidOperationException("Failed to create a reduction element.");
            }

            // Find price entry
            var priceEntry = reductionEle.Entries.SingleOrDefault(ti => ti.UnitPriceRelevance == ValueRelevance.Negative);
            if (priceEntry == null)
            {
                throw new InvalidOperationException("Could not find price entry to reduce / close position");
            }
            priceEntry.ContentWrapper = new ContentRecord(closingPrice);

            // Find cost entry
            var costEntry = reductionEle.Entries.SingleOrDefault(ti => ti.TotalCostRelevance == ValueRelevance.Negative);
            if (costEntry == null)
            {
                throw new InvalidOperationException("Could not find cost entry to reduce / close position");
            }

            if (double.TryParse(closingPrice, out double closingPriceValue))
            {
                var analytics = Analytics.GetTradingCosts(trade);
                double netAmountInPosition = analytics.addPositions.TotalAmount - analytics.reducePositions.TotalAmount;
                double costToClose = closingPriceValue * netAmountInPosition;
                costEntry.ContentWrapper = new ContentRecord(costToClose.ToF2String());
                trade.TradeElements.Add(reductionEle);

                trade.Summary = TradeElementsFactory.GetNewElement(trade, TradeActionType.Summary) as TradeSummary;
                trade.Close();
            }
            else
            {
                throw new FormatException("Could not parse closing price");
            }

            return trade;
        }
    }
}
