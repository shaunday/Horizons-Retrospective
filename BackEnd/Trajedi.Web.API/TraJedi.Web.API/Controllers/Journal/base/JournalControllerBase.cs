using Microsoft.AspNetCore.Mvc;
using TraJedi.Journal.Data.Services;

namespace TraJedi.Web.API.Controllers.Journal
{
    public class JournalControllerBase : ControllerBase
    {
        protected readonly ITradesRepository _journalAccess;
        protected readonly ILogger<JournalControllerBase> _logger;

        public JournalControllerBase(ITradesRepository journalAccess, ILogger<JournalControllerBase> logger)
        {
            _journalAccess = journalAccess ?? throw new ArgumentNullException(nameof(journalAccess));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected ActionResult ResultHandling(object? result, string logEntry)
        {
            if (result == null)
            {
                _logger.LogWarning(logEntry);
                return NotFound();
            }
            else
            {
                return Ok(result);
            }
        }
    }
}
