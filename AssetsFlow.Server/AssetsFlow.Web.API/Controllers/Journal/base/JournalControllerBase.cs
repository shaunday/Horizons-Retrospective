﻿using AutoMapper;
using HsR.Journal.DataContext;
using HsR.Web.API.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace HsR.Web.API.Controllers.Journal
{
    public class JournalControllerBase : HsRControllerBase
    {
        private protected readonly string NEW_CELL_DATA = "updatedCellData";
        private protected readonly string NEW_ELEMENT_DATA = "updatedElement";
        private protected readonly string NEW_SUMMARY = "updatedSummary";
        private protected readonly string NEW_TIMESTAMP = "updatedTimeStamp";

        private protected readonly IJournalRepositoryWrapper _journalAccess;
        private protected readonly ILogger<JournalControllerBase> _logger;
        private protected readonly IMapper _mapper;

        public JournalControllerBase(IJournalRepositoryWrapper journalAccess, ILogger<JournalControllerBase> logger, IMapper mapper)
        {
            _journalAccess = journalAccess ?? throw new ArgumentNullException(nameof(journalAccess));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        protected ActionResult ResultHandling(object? result, string logEntry, params string[] propertyNames)
        {
            if (result == null)
            {
                _logger.LogWarning(logEntry);
                return NotFound();
            }

            // If the result is a ValueTuple, extract its values into a dictionary
            if (result.GetType().Name.StartsWith("ValueTuple"))
            {
                var responseObject = new Dictionary<string, object?>();
                var props = result.GetType().GetFields();

                for (int i = 0; i < props.Length; i++)
                {
                    var value = props[i].GetValue(result);
                    responseObject[propertyNames.Length > i ? propertyNames[i] : $"item{i + 1}"] = value;
                }

                return Ok(responseObject);
            }

            // If it's a single class, return it directly without wrapping in a dictionary
            return Ok(result);
        }

    }
}
