using Microsoft.AspNetCore.Mvc;
using TraJedi.Journal.Data;

namespace TraJedi.Web.API.Controllers.Journal
{
    public class JournalControllerBase : ControllerBase
    {
        protected readonly TradingJournalAccess _journalAccess;

        public JournalControllerBase(TradingJournalAccess journalAccess)
        {
            _journalAccess = journalAccess ?? throw new ArgumentNullException(nameof(journalAccess));
        }
    }
}
