using HsR.Common;
using HsR.Journal.Entities;
using HsR.Journal.Entities.TradeJournal;
using HsR.Journal.Repository.Services.CompositeRepo;
using HsR.Journal.Services;

namespace HsR.Journal.DataContext
{
    public class JournalRepositoryWrapper : IJournalRepositoryWrapper
    {
        public IJournalRepository Journal { get; }
        public ITradeCompositeRepository TradeComposite { get; }
        public ITradeElementRepository TradeElement { get; }
        public IDataElementRepository DataElement { get; }

        public IUserDataRepository UserDataRepo { get; }

        public JournalRepositoryWrapper(
            IJournalRepository tradeCompositesRepository,
            ITradeCompositeRepository tradeCompositeRepository,
            ITradeElementRepository tradeElementRepository,
            IDataElementRepository dataElementRepository,
            IUserDataRepository userDataRepository)
        {
            Journal = tradeCompositesRepository;
            TradeComposite = tradeCompositeRepository;
            TradeElement = tradeElementRepository;
            DataElement = dataElementRepository;
            UserDataRepo = userDataRepository;
        }

    } 
}


