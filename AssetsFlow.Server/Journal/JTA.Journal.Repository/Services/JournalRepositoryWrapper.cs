using HsR.Common;
using HsR.Journal.Entities;

namespace HsR.Journal.DataContext
{
    public class JournalRepositoryWrapper : IJournalRepositoryWrapper
    {
        #region Members
        private readonly ITradeCompositesRepository _tradeCompositesRepository;
        private readonly ITradeElementRepository _tradeElementRepository;
        private readonly IDataElementRepository _dataElementRepository;
        private readonly IUserDataRepository _generalDataRepository;
        #endregion

        #region Ctor
        public JournalRepositoryWrapper(
            ITradeCompositesRepository tradeCompositesRepository,
            ITradeElementRepository tradeElementRepository,
            IDataElementRepository dataElementRepository,
            IUserDataRepository generalDataRepository)
        {
            _tradeCompositesRepository = tradeCompositesRepository;
            _tradeElementRepository = tradeElementRepository;
            _dataElementRepository = dataElementRepository;
            _generalDataRepository = generalDataRepository;
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


        public Task<(TradeElement newEntry, TradeElement summary)> AddInterimPositionAsync(string tradeId, bool isAdd)
        {
            return _tradeElementRepository.AddInterimPositionAsync(tradeId, isAdd);
        }

        public Task<TradeElement> RemoveInterimPositionAsync(string tradeId, string tradeInputId)
        {
            return _tradeElementRepository.RemoveInterimPositionAsync(tradeId, tradeInputId);
        }

        public Task<TradeElement> ActivateTradeElement(string tradeEleId)
        {
            return _tradeElementRepository.ActivateTradeElement(tradeEleId);
        }

        public Task<(DataElement updatedCell, TradeElement? summary)> UpdateCellContentAsync(string componentId, string newContent, string changeNote)
        {
            return _dataElementRepository.UpdateCellContentAsync(componentId, newContent, changeNote);
        }


        public Task<List<string>?> GetAllSavedSectors()
        {
            return _generalDataRepository.GetAllSavedSectors();
        }
    } 
}


