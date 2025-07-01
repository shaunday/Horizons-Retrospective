using HsR.Journal.DataContext;
using HsR.Journal.Entities;
using HsR.Journal.Entities.Factory;
using Microsoft.EntityFrameworkCore;
using HsR.Common.ContentGenerators;
using HsR.Common;
using System.Xml.Linq;
using HsR.Journal.Entities.Factory.Assists;
using HsR.Journal.Seeder;
using Microsoft.Extensions.Configuration;
using HsR.UserService.Contracts;

namespace HsR.Journal.Seeder
{
    public class DatabaseSeeder
    {
        private readonly TradingJournalDataContext dbContext;
        private readonly IJournalRepository _tradeCompositesRepository;
        private readonly ITradeElementRepository _tradeElementRepository;
        private readonly IDataElementRepository _dataElementRepository;
        private readonly Guid _demoUserId = Guid.Parse(DemoUserData.Id);

        public DatabaseSeeder(
            TradingJournalDataContext dbContext,
            IJournalRepository tradeCompositesRepository,
            ITradeElementRepository tradeElementRepository,
            IDataElementRepository dataElementRepository,
            IConfiguration configuration)
        {
            this.dbContext = dbContext;
            _tradeCompositesRepository = tradeCompositesRepository;
            _tradeElementRepository = tradeElementRepository;
            _dataElementRepository = dataElementRepository;
        }

        public async Task FlushDbAndSeedDemoAsync()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            await SeedDemoUserTradesAsync();
        }

        public async Task SeedDemoUserTradesAsync()
        {
            var trade1 = await _tradeCompositesRepository.AddTradeCompositeAsync(_demoUserId);
            await dbContext.SaveChangesAsync();
            await PopulateStageContent(_demoUserId, trade1, ManualDemoTrades.Trade1_Idea);

            var trade2 = await _tradeCompositesRepository.AddTradeCompositeAsync(_demoUserId);
            await dbContext.SaveChangesAsync();
            await PopulateStageContent(_demoUserId, trade2, ManualDemoTrades.Trade2_Idea);
            var add2_1 = (await _tradeElementRepository.AddInterimPositionAsync(_demoUserId, trade2.Id.ToString(), true)).newEntry;
            await dbContext.SaveChangesAsync();
            await PopulateStageContent(_demoUserId, add2_1, ManualDemoTrades.Trade2_Add1);
            var add2_2 = (await _tradeElementRepository.AddInterimPositionAsync(_demoUserId, trade2.Id.ToString(), true)).newEntry;
            await dbContext.SaveChangesAsync();
            await PopulateStageContent(_demoUserId, add2_2, ManualDemoTrades.Trade2_Add2);
            var eval2 = (await _tradeElementRepository.AddInterimEvalutationAsync(_demoUserId, trade2.Id.ToString())).newEntry;
            await dbContext.SaveChangesAsync();
            await PopulateStageContent(_demoUserId, eval2, ManualDemoTrades.Trade2_Evaluate);

            var trade3 = await _tradeCompositesRepository.AddTradeCompositeAsync(_demoUserId);
            await dbContext.SaveChangesAsync();
            await PopulateStageContent(_demoUserId, trade3, ManualDemoTrades.Trade3_Idea);
            var add3 = (await _tradeElementRepository.AddInterimPositionAsync(_demoUserId, trade3.Id.ToString(), true)).newEntry;
            await dbContext.SaveChangesAsync();
            await PopulateStageContent(_demoUserId, add3, ManualDemoTrades.Trade3_Add);
            var eval3 = (await _tradeElementRepository.AddInterimEvalutationAsync(_demoUserId, trade3.Id.ToString())).newEntry;
            await dbContext.SaveChangesAsync();
            await PopulateStageContent(_demoUserId, eval3, ManualDemoTrades.Trade3_Evaluate);
            trade3.Close();
            dbContext.TradeComposites.Update(trade3);
            await dbContext.SaveChangesAsync();
            var closeElement = trade3.TradeElements.LastOrDefault(e => e.TradeActionType.ToString() == "Summary");
            if (closeElement != null)
                await PopulateStageContent(_demoUserId, closeElement, ManualDemoTrades.Trade3_Close);
        }

        private async Task PopulateStageContent(Guid userId, TradeComposite trade, Dictionary<string, string> content)
        {
            foreach (var element in trade.TradeElements)
            {
                if (element == null)
                {
                    Console.WriteLine($"[Seeder Warning] TradeComposite {trade.Id} has a null TradeElement.");
                    continue;
                }
                await PopulateStageContent(userId, element, content);
            }
        }

        private async Task PopulateStageContent(Guid userId, TradeElement element, Dictionary<string, string> content)
        {
            foreach (var entry in element.Entries)
            {
                if (entry == null)
                {
                    Console.WriteLine($"[Seeder Warning] TradeElement {element.Id} has a null Entry.");
                    continue;
                }
                if (content.TryGetValue(entry.Title, out var value))
                {
                    try
                    {
                        await _dataElementRepository.UpdateCellContentAsync(userId, entry.Id.ToString(), value, "manual seed");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[Seeder Error] Failed to update EntryId {entry.Id} (Title: {entry.Title}): {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine($"[Seeder Warning] No content found for Entry Title '{entry.Title}' in TradeElement {element.Id}.");
                }
            }
        }

        public async Task DeleteAllDemoUserTradesAsync()
        {
            var demoTrades = dbContext.TradeComposites.Where(t => t.UserId == _demoUserId);
            dbContext.TradeComposites.RemoveRange(demoTrades);
            await dbContext.SaveChangesAsync();
        }
    }
}