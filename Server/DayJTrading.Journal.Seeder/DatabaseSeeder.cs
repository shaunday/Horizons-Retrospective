using DayJT.Journal.Data;
using DayJT.Journal.DataContext.Services;
using DayJTrading.Journal.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;

namespace DayJTrading.Journal.Seeder
{
    public class DatabaseSeeder
    {
        private readonly TradingJournalDataContext _context;

        private RandomNumberGenerator _randomNumbersMachine = new RandomNumberGenerator();
        private RandomWordsGenerator _randomWordsMachine = new RandomWordsGenerator();


        public DatabaseSeeder(TradingJournalDataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            // Check if any data exists in a specific table to avoid reseeding
            if (!await _context.AllTradeComposites.AnyAsync())
            {
                AddTradeCompositeToDbContext();
                await _context.SaveChangesAsync();
            }
        }

        private void AddTradeCompositeToDbContext()
        {
            TradeComposite trade = new TradeComposite();
            TradeElement originElement = new TradeElement(trade, TradeActionType.Origin);
            PopulateElementWithData(originElement);
            trade.TradeElements.Add(originElement);

            TradeElement addElement = new TradeElement(trade, TradeActionType.AddPosition);
            PopulateElementWithData(addElement);
            trade.TradeElements.Add(addElement);

            _context.AllTradeComposites.Add(trade);
        }

        private static readonly Random _lengthRandom = new Random();
        private TradeElement PopulateElementWithData(TradeElement element)
        {
            int length;
            for (int i = 0; i < element.Entries.Count; i++)
            {
                length = _lengthRandom.Next(3, 8);
                if (element.Entries[i].CostRelevance != ValueRelevance.None || element.Entries[i].PriceRelevance != ValueRelevance.None)
                {
                    element.Entries[i].Content = _randomNumbersMachine.GenerateRandomNumber(length);
                }
                else
                {
                    {
                        element.Entries[i].Content = _randomWordsMachine.GenerateRandomWord(length);
                    }
                }
            }

            return element;
        }
    }
}