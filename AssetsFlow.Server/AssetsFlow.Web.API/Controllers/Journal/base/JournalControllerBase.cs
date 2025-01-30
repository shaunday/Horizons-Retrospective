using AutoMapper;
using HsR.Journal.DataContext;
using HsR.Web.API.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace HsR.Web.API.Controllers.Journal
{
    internal class JournalControllerBase : HsRControllerBase
    {
        protected readonly IJournalRepositoryWrapper _journalAccess;
        protected readonly ILogger<JournalControllerBase> _logger;
        protected readonly IMapper _mapper;

        internal JournalControllerBase(IJournalRepositoryWrapper journalAccess, ILogger<JournalControllerBase> logger, IMapper mapper)
        {
            _journalAccess = journalAccess ?? throw new ArgumentNullException(nameof(journalAccess));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        protected ActionResult ResultHandling(object? result, string logEntry)
        {
            if (result == null || (result is Tuple<object, object> tuple && (tuple.Item1 == null || tuple.Item2 == null)))
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
