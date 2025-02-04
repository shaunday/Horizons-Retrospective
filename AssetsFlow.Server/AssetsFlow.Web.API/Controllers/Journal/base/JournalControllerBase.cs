﻿using AutoMapper;
using HsR.Journal.DataContext;
using HsR.Web.API.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace HsR.Web.API.Controllers.Journal
{
    public class JournalControllerBase : HsRControllerBase
    {
        private protected readonly IJournalRepositoryWrapper _journalAccess;
        private protected readonly ILogger<JournalControllerBase> _logger;
        private protected readonly IMapper _mapper;

        public JournalControllerBase(IJournalRepositoryWrapper journalAccess, ILogger<JournalControllerBase> logger, IMapper mapper)
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
