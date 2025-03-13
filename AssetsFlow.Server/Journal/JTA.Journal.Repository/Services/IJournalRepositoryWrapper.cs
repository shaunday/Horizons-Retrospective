using HsR.Common;
using HsR.Journal.Entities;
using HsR.Journal.Entities.TradeJournal;
using HsR.Journal.Repository.Services.CompositeRepo;
using HsR.Journal.Services;
using System.Data;

namespace HsR.Journal.DataContext
{
    public interface IJournalRepositoryWrapper
    {
        public IJournalRepository Journal { get; }
        public ITradeCompositeRepository TradeComposite { get; }
        public ITradeElementRepository TradeElement { get; }
        public IDataElementRepository DataElement { get; }

        public IUserDataRepository UserDataRepo { get; }
    }
}
