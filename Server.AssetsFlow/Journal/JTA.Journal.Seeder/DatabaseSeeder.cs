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
            if (!await context.TradeComposites.AnyAsync())
            {
                // Drop the database if it exists
                context.Database.EnsureDeleted();

                // Recreate the database
                context.Database.EnsureCreated();

                AddTradeIdeaToDbContext(context);

                AddOngoingTradeToDbContext(context);
                AddOngoingTradeToDbContext(context);

                //AddClosedTradeToDbContext(context);

                await context.SaveChangesAsync();
            }
        }

        private static void AddTradeIdeaToDbContext(TradingJournalDataContext context)
        {
            TradeComposite trade = GetTradeIdea();
            context.TradeComposites.Add(trade);
        }

        private static void AddOngoingTradeToDbContext(TradingJournalDataContext context)
        {
            TradeComposite trade = GetOngoingTrade();
            context.TradeComposites.Add(trade);
        }

        private static void AddClosedTradeToDbContext(TradingJournalDataContext context)
        {
            TradeComposite trade = GetOngoingTrade();
            TradeCompositeUpdates.CloseTrade(trade, "1000");

            context.TradeComposites.Add(trade);
        }

        private static TradeComposite GetOngoingTrade()
        {
            TradeComposite trade = GetTradeIdea();

            AddElementToTrade(trade, TradeActionType.AddPosition);
            AddElementToTrade(trade, TradeActionType.AddPosition);
            AddElementToTrade(trade, TradeActionType.ReducePosition);

            TradeCompositeUpdates.RecreateSummary(trade);
            return trade;
        }

        private static TradeComposite GetTradeIdea()
        {
            TradeComposite trade = new();
            TradeElement originElement = new(trade, TradeActionType.Origin);
            originElement.Entries = EntriesFactory.GetOriginEntries(originElement);
            PopulateElementWithData(originElement);
            trade.TradeElements.Add(originElement);
            return trade;
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