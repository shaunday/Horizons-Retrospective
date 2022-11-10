using Microsoft.AspNetCore.Mvc;
using TraJedi.Journal.Data;

namespace TraJedi.Web.API.Controllers.Journal
{
    public class JournalControllerBase : ControllerBase
    {
        protected readonly TradingJournalAccess _journalAccess;
        protected readonly ILogger<JournalControllerBase> _logger;

        public JournalControllerBase(TradingJournalAccess journalAccess, ILogger<JournalControllerBase> logger)
        {
            _journalAccess = journalAccess ?? throw new ArgumentNullException(nameof(journalAccess));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected virtual ActionResult ExceptionHandling(Exception ex, string cause)
        {
            _logger.LogCritical($"Exception while {cause}. {ex}");
            return StatusCode(500, "A problem happened while handling your request");
        }
    }
}
