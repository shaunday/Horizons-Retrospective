using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using DayJT.Journal.Data.Services;

namespace DayJT.Web.API.Controllers.Journal
{
    public class JournalControllerBase : ControllerBase
    {
        protected readonly IJournalRepository _journalAccess;
        protected readonly ILogger<JournalControllerBase> _logger;
        protected readonly IMapper _mapper;

        public JournalControllerBase(IJournalRepository journalAccess, ILogger<JournalControllerBase> logger, IMapper mapper)
        {
            _journalAccess = journalAccess ?? throw new ArgumentNullException(nameof(journalAccess));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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
