﻿using HsR.Journal.DataContext;
using HsR.Journal.Entities.Factory;
using HsR.Journal.Repository.Services.Base;
using HsR.Journal.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HsR.Journal.Repository.Services.CompositeRepo
{
    public class TradeCompositeRepository(TradingJournalDataContext dataContext) : JournalRepositoryBase(dataContext), ITradeCompositeRepository
    {
        public async Task<TradeComposite> CloseTradeAsync(int tradeId, string closingPrice)
        {
            var trade = await GetTradeCompositeAsync(tradeId);

            if (trade?.Summary != null)
            {
                _dataContext.Entry(trade.Summary).State = EntityState.Deleted;
            }
            else
            {
                throw new InvalidOperationException("Trade / summary is missing.");
            }
            
            TradeCompositeOperations.CloseTrade(trade, closingPrice);

            await _dataContext.SaveChangesAsync();
            return trade;
        }

        public async Task RefreshSaveSummaryAsync(TradeComposite trade)
        {
            _ = RefreshSummary(trade);
            await _dataContext.SaveChangesAsync();
        }
    }
}
