using HsR.Common;
using HsR.Journal.Entities;
using HsR.Journal.Entities.TradeJournal;

namespace HsR.Journal.DataContext
{
    public class JournalRepositoryWrapper : IJournalRepositoryWrapper
    {
        #region Members
        private readonly ITradeCompositesRepository _tradeCompositesRepository;
        private readonly ITradeElementRepository _tradeElementRepository;
        private readonly IDataElementRepository _dataElementRepository;
        private readonly IUserDataRepository _userDataRepository;
        #endregion

        #region Ctor
        public JournalRepositoryWrapper(
            ITradeCompositesRepository tradeCompositesRepository,
            ITradeElementRepository tradeElementRepository,
            IDataElementRepository dataElementRepository,
            IUserDataRepository userDataRepository)
        {
            _tradeCompositesRepository = tradeCompositesRepository;
            _tradeElementRepository = tradeElementRepository;
            _dataElementRepository = dataElementRepository;
            _userDataRepository = userDataRepository;
        } 
        #endregion

        public Task<(IEnumerable<TradeComposite>, PaginationMetadata)> GetAllTradeCompositesAsync(int pageNumber = 1, int pageSize = 10)
        {
            return _tradeCompositesRepository.GetAllTradeCompositesAsync(pageNumber, pageSize);
        }

        public Task<(IEnumerable<TradeComposite>, PaginationMetadata)> GetFilteredTradesAsync(TradesFilterModel filter, int pageNumber = 1, int pageSize = 10)
        {
            return _tradeCompositesRepository.GetFilteredTradesAsync(filter, pageNumber, pageSize);
        }

        public Task<TradeComposite> AddTradeCompositeAsync()
        {
            return _tradeCompositesRepository.AddTradeCompositeAsync();
        }

        public Task<TradeElement> CloseTradeAsync(string tradeId, string closingPrice)
        {
            return _tradeCompositesRepository.CloseTradeAsync(tradeId, closingPrice);
        }


        public Task<(TradeAction newEntry, TradeSummary summary)> AddInterimPositionAsync(string tradeId, bool isAdd)
        {
            return _tradeElementRepository.AddInterimPositionAsync(tradeId, isAdd);
        }

        public Task<TradeSummary> RemoveInterimPositionAsync(string tradeId, string tradeInputId)
        {
            return _tradeElementRepository.RemoveInterimPositionAsync(tradeId, tradeInputId);
        }

        public Task<TradeAction> ActivateTradeElement(string tradeEleId)
        {
            return _tradeElementRepository.ActivateTradeElement(tradeEleId);
        }

        public Task<(DataElement updatedCell, TradeSummary? summary)> UpdateCellContentAsync(string componentId, string newContent, string changeNote)
        {
            return _dataElementRepository.UpdateCellContentAsync(componentId, newContent, changeNote);
        }


        public Task<List<string>?> GetAllSavedSectors()
        {
            return _userDataRepository.GetAllSavedSectors();
        }
    } 
}


