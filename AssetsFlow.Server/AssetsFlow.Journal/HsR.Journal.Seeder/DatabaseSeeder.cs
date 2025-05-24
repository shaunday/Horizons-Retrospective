using HsR.Journal.DataContext;
using HsR.Journal.Entities;
using HsR.Journal.Entities.Factory;
using Microsoft.EntityFrameworkCore;
using HsR.Common.ContentGenerators;
using HsR.Common;
using System.Xml.Linq;

namespace HsR.Journal.Seeder
{
    public class DatabaseSeeder(TradingJournalDataContext dbContext, IJournalRepository tradeCompositesRepository,
            ITradeElementRepository tradeElementRepository,
            IDataElementRepository dataElementRepository)
    {

        #region Members
        private RandomNumberGeneratorEx _randomNumbersMachine = new();
        private RandomWordGenerator _randomWordsMachine = new();

        private readonly IJournalRepository _tradeCompositesRepository = tradeCompositesRepository;
        private readonly ITradeElementRepository _tradeElementRepository = tradeElementRepository;
        private readonly IDataElementRepository _dataElementRepository = dataElementRepository;
        #endregion

        public async Task SeedAsync()
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

                //ongoing
                trade = await CreateAndSaveTradeInstance();
                await AddManagePositions(trade);

                //closed
                trade = await CreateAndSaveTradeInstance();
                await AddManagePositions(trade, close: true);
            }
        }

        private async Task<TradeComposite> CreateAndSaveTradeInstance()
        {
            TradeComposite trade = new();
            dbContext.TradeComposites.Add(trade);
            await dbContext.SaveChangesAsync();

            AddTradeIdea(trade);
            trade.Status = TradeStatus.AnIdea;
            await dbContext.SaveChangesAsync();
            return trade;
        }

        private async Task AddManagePositions(TradeComposite trade, bool close = false)
        {
            AddElementToTrade(trade, TradeActionType.Add);
            AddElementToTrade(trade, TradeActionType.Add);
            AddElementToTrade(trade, TradeActionType.Reduce);


            if (close)
            {
                TradeCompositeOperations.CloseTrade(trade, "1000");
                trade.Status = TradeStatus.Closed;
            }
            else
            {
                var newSummary = TradeElementsFactory.GetNewSummary(trade);
                trade.Summary = newSummary;
                trade.Status = TradeStatus.Open;
            }

            dbContext.TradeComposites.Update(trade);
            await dbContext.SaveChangesAsync();
        }

        private void AddTradeIdea(TradeComposite trade)
        {
            InterimTradeElement originElement = new(trade, TradeActionType.Origin);
            originElement.Entries = EntriesFactory.GetOriginEntries(originElement);
            PopulateElementWithData(originElement);
            trade.TradeElements.Add(originElement);
        }

        private void AddElementToTrade(TradeComposite trade, TradeActionType type)
        {
            InterimTradeElement newElement = new(trade, type);
            newElement.Entries = EntriesFactory.GetAddPositionEntries(newElement);

            PopulateElementWithData(newElement);
            newElement.Activate();
            trade.TradeElements.Add(newElement);
        }

        //populate related
        private readonly Random _lengthRandom = new();
        private InterimTradeElement PopulateElementWithData(InterimTradeElement element)
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