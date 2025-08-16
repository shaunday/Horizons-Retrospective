using HsR.Common;
using HsR.Journal.Entities;
using HsR.Journal.Entities.TradeJournal;
using HsR.Journal.Repository.Services.CompositeRepo;
using HsR.Journal.Services;

namespace HsR.Journal.DataContext
{
    public class JournalRepositoryWrapper(
        IJournalRepository tradeCompositesRepository,
        ITradeCompositeRepository tradeCompositeRepository,
        ITradeElementRepository tradeElementRepository,
        IDataElementRepository dataElementRepository,
        IUserDataRepository userDataRepository) : IJournalRepositoryWrapper
    {
        public IJournalRepository Journal { get; } = tradeCompositesRepository;
        public ITradeCompositeRepository TradeComposite { get; } = tradeCompositeRepository;
        public ITradeElementRepository TradeElement { get; } = tradeElementRepository;
        public IDataElementRepository DataElement { get; } = dataElementRepository;

        public IUserDataRepository UserDataRepo { get; } = userDataRepository;
    } 
}


