using DayJT.Journal.Data;
using DayJT.Journal.DataEntities.Entities;
using DayJT.Journal.Repository;
using DayJTrading.Journal.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;

namespace DayJTrading.Journal.Seeder
{
    internal static class DatabaseSeeder
    {
        private static RandomNumberGenerator _randomNumbersMachine = new();
        private static RandomWordsGenerator _randomWordsMachine = new();

        internal static async Task SeedAsync(TradingJournalDataContext context)
        {
            // Check if any data exists in a specific table to avoid reseeding
            if (!await context.TradeComposites.AnyAsync())
            {
                AddTradeCompositeToDbContext(context);
                await context.SaveChangesAsync();
            }
        }

        private static void AddTradeCompositeToDbContext(TradingJournalDataContext context)
        {
            TradeComposite trade = new();
            TradeElement originElement = new(trade, TradeActionType.Origin);
            PopulateElementWithData(originElement);
            trade.TradeElements.Add(originElement);

            TradeElement addElement = new(trade, TradeActionType.AddPosition);
            PopulateElementWithData(addElement);
            trade.TradeElements.Add(addElement);

            context.TradeComposites.Add(trade);
        }

        private static readonly Random _lengthRandom = new();
        private static TradeElement PopulateElementWithData(TradeElement element)
        {
            int length;
            for (int i = 0; i < element.Entries.Count; i++)
            {
                length = _lengthRandom.Next(3, 8);
                if (element.Entries[i].CostRelevance != null || element.Entries[i].PriceRelevance != null)
                {
                    element.Entries[i].ContentWrapper = new ContentRecord(_randomNumbersMachine.GenerateRandomNumber(length));
                }
                else
                {
                    {
                        element.Entries[i].ContentWrapper = new ContentRecord(_randomWordsMachine.GenerateRandomWord(length));
                    }
                }
            }

            return element;
        }
    }
}