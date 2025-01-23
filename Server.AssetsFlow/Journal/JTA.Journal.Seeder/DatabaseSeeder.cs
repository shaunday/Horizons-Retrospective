using HsR.Journal.DataContext;
using HsR.Journal.Entities;
using HsR.Journal.Entities.Factory;
using Microsoft.EntityFrameworkCore;
using HsR.Common.ContentGenerators;

namespace HsR.Journal.Seeder
{
    internal static class DatabaseSeeder
    {
        private static RandomNumberGeneratorEx _randomNumbersMachine = new();
        private static RandomWordGenerator _randomWordsMachine = new();

        internal static async Task SeedAsync(TradingJournalDataContext context)
        {
            // Check if any data exists in a specific table to avoid reseeding
            //if (!await context.TradeComposites.AnyAsync())
            {
                // Drop the database if it exists
                context.Database.EnsureDeleted();

                // Recreate the database
                context.Database.EnsureCreated();

                //idea
                TradeComposite trade = await CreateAndSaveTradeInstance(context);

                //ongoing
                trade = await CreateAndSaveTradeInstance(context);
                AddPositionsAndSummary(trade);
                await context.SaveChangesAsync();

                //closed
                trade = await CreateAndSaveTradeInstance(context);
                AddPositionsAndSummary(trade);
                TradeCompositeUpdates.CloseTrade(trade, "1000");
                await context.SaveChangesAsync();
            }
        }

        private static async Task<TradeComposite> CreateAndSaveTradeInstance(TradingJournalDataContext context)
        {
            TradeComposite trade = new();
            context.TradeComposites.Add(trade);
            await context.SaveChangesAsync();
            AddTradeIdea(trade);
            await context.SaveChangesAsync();
            return trade;
        }

        private static void AddPositionsAndSummary(TradeComposite trade)
        {
            AddElementToTrade(trade, TradeActionType.AddPosition);
            AddElementToTrade(trade, TradeActionType.AddPosition);
            AddElementToTrade(trade, TradeActionType.ReducePosition);

            TradeCompositeUpdates.RecreateSummary(trade);
        }

        private static void AddTradeIdea(TradeComposite trade)
        {
            TradeElement originElement = new(trade, TradeActionType.Origin);
            originElement.Entries = EntriesFactory.GetOriginEntries(originElement);
            PopulateElementWithData(originElement);
            trade.TradeElements.Add(originElement);
        }

        private static void AddElementToTrade(TradeComposite trade, TradeActionType type)
        {
            TradeElement addElement = new(trade, type);
            addElement.Entries = EntriesFactory.GetAddPositionEntries(addElement);

            PopulateElementWithData(addElement);
            trade.TradeElements.Add(addElement);
        }

        //populate related
        private static readonly Random _lengthRandom = new();
        private static TradeElement PopulateElementWithData(TradeElement element)
        {
            int length;
            for (int i = 0; i < element.Entries.Count; i++)
            {
                length = _lengthRandom.Next(3, 8);
                if (element.Entries[i].IsCostRelevant)
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