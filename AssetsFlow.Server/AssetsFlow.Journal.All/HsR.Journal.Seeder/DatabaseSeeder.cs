using HsR.Common;
using HsR.Common.ContentGenerators;
using HsR.Journal.DataContext;
using HsR.Journal.Entities;
using HsR.Journal.Entities.Factory;
using HsR.Journal.Entities.TradeJournal;
using HsR.Journal.Repository.Services.CompositeRepo;
using HsR.Journal.Seeder;
using HsR.UserService.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Xml.Linq;

namespace HsR.Journal.Seeder
{
    public class DatabaseSeeder
    {
        #region Fields and Constructor

        private readonly TradingJournalDataContext dbContext;
        private readonly IJournalRepository _journalRepository;
        private readonly ITradeCompositeRepository _tradeCompositeRepository;
        private readonly ITradeElementRepository _tradeElementRepository;
        private readonly IDataElementRepository _dataElementRepository;
        private readonly Guid _demoUserId = Guid.Parse(DemoUserData.Id);

        public DatabaseSeeder(
            TradingJournalDataContext dbContext,
            IJournalRepository journalRepository,
            ITradeCompositeRepository tradeCompositeRepository,
            ITradeElementRepository tradeElementRepository,
            IDataElementRepository dataElementRepository,
            IConfiguration configuration)
        {
            this.dbContext = dbContext;
            _journalRepository = journalRepository;
            _tradeCompositeRepository = tradeCompositeRepository;
            _tradeElementRepository = tradeElementRepository;
            _dataElementRepository = dataElementRepository;
        }

        #endregion

        #region Public Methods

        public async Task FlushDbAndSeedDemoAsync()
        {
            await dbContext.Database.EnsureDeletedAsync(); //dont ocmmit this ye

            await dbContext.Database.EnsureCreatedAsync();

            await SeedDemoUserTradesAsync();
        }

        public async Task SeedDemoUserTradesAsync()
        {
            await SeedTrade1Async();
            await SeedTrade2Async();
            await SeedTrade3Async();
            await SeedTrade4Async();
            await SeedTrade5Async();
            await SeedTrade6Async();

            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAllDemoUserTradesAsync()
        {
            // 1. Null out circular references
            var summaries = await dbContext.TradeElements
                .OfType<TradeSummary>()
                .Where(s => s.UserId == _demoUserId)
                .ToListAsync();

            foreach (var summary in summaries)
            {
                summary.CompositeRef = null!;
            }

            var composites = await dbContext.TradeComposites
                .Where(c => c.UserId == _demoUserId)
                .ToListAsync();

            foreach (var composite in composites)
            {
                composite.Summary = null!;
            }

            await dbContext.SaveChangesAsync(); // Apply nulls to break cycles

            // 2. Delete in FK-safe order: Entries → Elements → Composites
            var entries = dbContext.Entries.Where(e => e.UserId == _demoUserId);
            dbContext.Entries.RemoveRange(entries);

            var elements = dbContext.TradeElements.Where(e => e.UserId == _demoUserId);
            dbContext.TradeElements.RemoveRange(elements);

            dbContext.TradeComposites.RemoveRange(composites); // already queried above

            await dbContext.SaveChangesAsync();
        }

        #endregion

        #region Private Seed Methods

        private async Task SeedTrade1Async()
        {
            var trade1 = await _journalRepository.AddTradeCompositeAsync(_demoUserId);
            await PopulateStageContent(trade1, ManualDemoTrades.Trade1_Idea);
        }

        private async Task SeedTrade2Async()
        {
            var trade2 = await _journalRepository.AddTradeCompositeAsync(_demoUserId);
            await PopulateStageContent(trade2, ManualDemoTrades.Trade2_Idea);

            var add2_1 = await _tradeElementRepository.AddInterimPositionAsync(trade2.Id, true);
            await PopulateStageContent(add2_1, ManualDemoTrades.Trade2_Add1);

            var add2_2 = await _tradeElementRepository.AddInterimPositionAsync(trade2.Id, true);
            await PopulateStageContent(add2_2, ManualDemoTrades.Trade2_Add2);

            await _tradeCompositeRepository.RefreshSaveSummaryAsync(trade2);

            var eval2 = await _tradeElementRepository.AddInterimEvalutationAsync(trade2.Id);
            await PopulateStageContent(eval2, ManualDemoTrades.Trade2_Evaluate);
        }

        private async Task SeedTrade3Async()
        {
            var trade3 = await _journalRepository.AddTradeCompositeAsync(_demoUserId);
            await PopulateStageContent(trade3, ManualDemoTrades.Trade3_Idea);

            var add3 = await _tradeElementRepository.AddInterimPositionAsync(trade3.Id, true);
            await PopulateStageContent(add3, ManualDemoTrades.Trade3_Add);

            await _tradeCompositeRepository.RefreshSaveSummaryAsync(trade3);

            var eval3 = await _tradeElementRepository.AddInterimEvalutationAsync(trade3.Id);
            await PopulateStageContent(eval3, ManualDemoTrades.Trade3_Evaluate);

            TradeCompositeOperations.CloseTrade(trade3, "1250");

            await PopulateStageContent(trade3.Summary!, ManualDemoTrades.Trade3_Close);
        }

        private async Task SeedTrade4Async()
        {
            var trade4 = await _journalRepository.AddTradeCompositeAsync(_demoUserId);
            await PopulateStageContent(trade4, ManualDemoTrades.Trade4_Idea);

            var add4 = await _tradeElementRepository.AddInterimPositionAsync(trade4.Id, true);
            await PopulateStageContent(add4, ManualDemoTrades.Trade4_Add1);

            await _tradeCompositeRepository.RefreshSaveSummaryAsync(trade4);

            var eval4 = await _tradeElementRepository.AddInterimEvalutationAsync(trade4.Id);
            await PopulateStageContent(eval4, ManualDemoTrades.Trade4_Evaluate);

            TradeCompositeOperations.CloseTrade(trade4, "900");

            await PopulateStageContent(trade4.Summary!, ManualDemoTrades.Trade4_Close);
        }

        private async Task SeedTrade5Async()
        {
            var trade5 = await _journalRepository.AddTradeCompositeAsync(_demoUserId);
            await PopulateStageContent(trade5, ManualDemoTrades.Trade5_Idea);

            var add5_1 = await _tradeElementRepository.AddInterimPositionAsync(trade5.Id, true);
            await PopulateStageContent(add5_1, ManualDemoTrades.Trade5_Add1);

            await _tradeCompositeRepository.RefreshSaveSummaryAsync(trade5);

            var eval5 = await _tradeElementRepository.AddInterimEvalutationAsync(trade5.Id);
            await PopulateStageContent(eval5, ManualDemoTrades.Trade5_Evaluate);
        }

        private async Task SeedTrade6Async()
        {
            var trade6 = await _journalRepository.AddTradeCompositeAsync(_demoUserId);
            await PopulateStageContent(trade6, ManualDemoTrades.Trade6_Idea);

            var add6 = await _tradeElementRepository.AddInterimPositionAsync(trade6.Id, true);
            await PopulateStageContent(add6, ManualDemoTrades.Trade6_Add);

            await _tradeCompositeRepository.RefreshSaveSummaryAsync(trade6);

            var eval6 = await _tradeElementRepository.AddInterimEvalutationAsync(trade6.Id);
            await PopulateStageContent(eval6, ManualDemoTrades.Trade6_Evaluate);

            TradeCompositeOperations.CloseTrade(trade6, "1500");

            await PopulateStageContent(trade6.Summary!, ManualDemoTrades.Trade6_Close);
        }

        #endregion

        #region Private Helper Methods

        private async Task PopulateStageContent(TradeComposite trade, Dictionary<string, string> content)
        {
            foreach (var element in trade.TradeElements)
            {
                if (element == null)
                {
                    Console.WriteLine($"[Seeder Warning] TradeComposite {trade.Id} has a null TradeElement.");
                    continue;
                }
                await PopulateStageContent(element, content);
            }
        }

        private async Task PopulateStageContent(TradeElement element, Dictionary<string, string> content)
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
                        await _dataElementRepository.UpdateCellContentAsync(entry.Id, new Services.UpdateDataComponentRequest() { Content = value });
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

        #endregion
    }
}
