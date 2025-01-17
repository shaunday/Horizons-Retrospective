using HsR.Common;
using HsR.Journal.Entities;

namespace HsR.Journal.DataContext
{
    public abstract class JournalRepositoryBase
    {
        protected readonly TradingJournalDataContext _dataContext;

        protected JournalRepositoryBase(TradingJournalDataContext dataContext)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        }
    }
}
