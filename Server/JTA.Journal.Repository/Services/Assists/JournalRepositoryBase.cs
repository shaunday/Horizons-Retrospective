using JTA.Common;
using JTA.Journal.Entities;

namespace JTA.Journal.DataContext
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
