using HsR.Journal.DataContext;
using HsR.Journal.Entities;
using HsR.Journal.Entities.Factory;
using Microsoft.EntityFrameworkCore;
using HsR.Common.ContentGenerators;
using HsR.Common;
using System.Xml.Linq;

namespace HsR.Journal.Seeder
{
    internal class DatabaseSeeder(TradingJournalDataContext dbContext)
    {
        private RandomNumberGeneratorEx _randomNumbersMachine = new();
        private RandomWordGenerator _randomWordsMachine = new();

        internal async Task SeedAsync()
        {
            // Check if any data exists in a specific table to avoid reseeding
           // if (!await _dbContext.TradeComposites.AnyAsync())
            {
                // Drop the database if it exists
                dbContext.Database.EnsureDeleted();

                // Recreate the database
                dbContext.Database.EnsureCreated();

                //idea
                TradeComposite trade = await CreateAndSaveTradeInstance();
                dbContext.TradeComposites.Update(trade); 

                //ongoing
                trade = await CreateAndSaveTradeInstance();
                AddPositionsAndSummary(trade);
                dbContext.TradeComposites.Update(trade);

                //closed
                trade = await CreateAndSaveTradeInstance();
                AddPositionsAndSummary(trade);
                TradeCompositeUpdates.CloseTrade(trade, "1000");
                dbContext.TradeComposites.Update(trade);

                await dbContext.SaveChangesAsync();
            }
        }

        private async Task<TradeComposite> CreateAndSaveTradeInstance()
        {
            TradeComposite trade = new();
            dbContext.TradeComposites.Add(trade);
            await dbContext.SaveChangesAsync();

            AddTradeIdea(trade);
            await dbContext.SaveChangesAsync();
            return trade;
        }

        private void AddPositionsAndSummary(TradeComposite trade)
        {
            AddElementToTrade(trade, TradeActionType.AddPosition);
            AddElementToTrade(trade, TradeActionType.AddPosition);
            AddElementToTrade(trade, TradeActionType.ReducePosition);

            TradeElement newSummary = TradeElementCRUDs.GetInterimSummary(trade);
            trade.Summary = newSummary;
        }

        private void AddTradeIdea(TradeComposite trade)
        {
            TradeElement originElement = new(trade, TradeActionType.Origin);
            originElement.Entries = EntriesFactory.GetOriginEntries(originElement);
            PopulateElementWithData(originElement);
            trade.TradeElements.Add(originElement);
        }

        private void AddElementToTrade(TradeComposite trade, TradeActionType type)
        {
            TradeElement newElement = new(trade, type);
            newElement.Entries = EntriesFactory.GetAddPositionEntries(newElement);

            PopulateElementWithData(newElement);
            trade.TradeElements.Add(newElement);
        }

        //populate related
        private readonly Random _lengthRandom = new();
        private TradeElement PopulateElementWithData(TradeElement element)
        {
            int length;
            for (int i = 0; i < element.Entries.Count; i++)
            {
                if (element.Entries[i].Restrictions != null)
                {
                    continue;
                }
                length = _lengthRandom.Next(3, 8);
                if (element.Entries[i].IsCostRelevant())
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