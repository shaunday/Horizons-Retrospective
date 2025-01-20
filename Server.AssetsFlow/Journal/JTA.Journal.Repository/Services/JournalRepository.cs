using HsR.Common;
using HsR.Journal.Entities;

namespace HsR.Journal.DataContext
{
    public class JournalRepository : IJournalRepository
    {
        #region Members
        private readonly ITradeCompositeRepository _tradeCompositeRepository;
        private readonly ITradeElementRepository _tradeElementRepository;
        private readonly IDataElementRepository _dataElementRepository;
        private readonly IGeneralDataRepository _generalDataRepository;
        #endregion

        #region ctor
        public JournalRepository(
            ITradeCompositeRepository tradeCompositeRepository,
            ITradeElementRepository tradeElementRepository,
            IDataElementRepository dataElementRepository,
            IGeneralDataRepository generalDataRepository)
        {
            _tradeCompositeRepository = tradeCompositeRepository;
            _tradeElementRepository = tradeElementRepository;
            _dataElementRepository = dataElementRepository;
            _generalDataRepository = generalDataRepository;
        } 
        #endregion

        public Task<(IEnumerable<TradeComposite>, PaginationMetadata)> GetAllTradeCompositesAsync(int pageNumber = 1, int pageSize = 10)
        {
            return _tradeCompositeRepository.GetAllTradeCompositesAsync(pageNumber, pageSize);
        }

        public Task<(IEnumerable<TradeComposite>, PaginationMetadata)> GetFilteredTradesAsync(TradesFilterModel filter, int pageNumber = 1, int pageSize = 10)
        {
            return _tradeCompositeRepository.GetFilteredTradesAsync(filter, pageNumber, pageSize);
        }

        public Task<TradeComposite> AddTradeCompositeAsync()
        {
            return _tradeCompositeRepository.AddTradeCompositeAsync();
        }

        public Task<TradeElement> CloseTradeAsync(string tradeId, string closingPrice)
        {
            return _tradeElementRepository.CloseTradeAsync(tradeId, closingPrice);
        }


        public Task<(TradeElement newEntry, TradeElement summary)> AddInterimPositionAsync(string tradeId, bool isAdd)
        {
            return _tradeElementRepository.AddInterimPositionAsync(tradeId, isAdd);
        }

        public Task<TradeElement> RemoveInterimPositionAsync(string tradeId, string tradeInputId)
        {
            return _tradeElementRepository.RemoveInterimPositionAsync(tradeId, tradeInputId);
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


