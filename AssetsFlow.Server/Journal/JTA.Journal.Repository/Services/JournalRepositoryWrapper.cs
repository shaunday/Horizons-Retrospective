using HsR.Common;
using HsR.Journal.Entities;
using HsR.Journal.Entities.TradeJournal;
using HsR.Journal.Services;

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

        #region Composite
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
        #endregion

        #region Element
        public Task<(InterimTradeElement newEntry, UpdatedStatesCollation? updatedStates)> AddInterimPositionAsync(string tradeId, bool isAdd)
        {
            return _tradeElementRepository.AddInterimPositionAsync(tradeId, isAdd);
        }

        public Task<InterimTradeElement> AddInterimEvalutationAsync(string tradeId)
        {
            return _tradeElementRepository.AddInterimEvalutationAsync(tradeId);
        }

        public Task<UpdatedStatesCollation> CloseTradeAsync(string tradeId, string closingPrice)
        {
            return _tradeElementRepository.CloseTradeAsync(tradeId, closingPrice);
        }

        public Task<UpdatedStatesCollation> RemoveInterimPositionAsync(string tradeInputId)
        {
            return _tradeElementRepository.RemoveInterimPositionAsync(tradeInputId);
        } 
        #endregion

        public Task<(DataElement updatedCell, UpdatedStatesCollation updatedStates)> 
                                                UpdateCellContentAsync(string componentId, string newContent, string changeNote)
        {
            return _dataElementRepository.UpdateCellContentAsync(componentId, newContent, changeNote);
        }


        public Task<List<string>?> GetAllSavedSectors()
        {
            return _userDataRepository.GetAllSavedSectors();
        }
    } 
}


