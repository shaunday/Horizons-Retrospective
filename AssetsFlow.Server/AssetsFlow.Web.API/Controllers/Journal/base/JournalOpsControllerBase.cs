using AutoMapper;
using HsR.Journal.DataContext;
using HsR.Journal.Repository.Services.Base;
using HsR.Web.API.Controllers;
using HsR.Web.API.Services;
using HsR.Web.Services.Models.Journal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HsR.Web.API.Controllers.Journal
{
    public class JournalOpsControllerBase : JournalControllerBase
    {
        private protected readonly string NEW_CELL_DATA = "updatedCellData";
        private protected readonly string NEW_ELEMENT_DATA = "updatedElement";
        private protected readonly string NEW_STATES_WRAPPER = "updatedStates";

        private protected readonly ITradesCacheService _cacheService;

        public JournalOpsControllerBase(
            IJournalRepositoryWrapper journalAccess, 
            ILogger<JournalOpsControllerBase> logger, 
            IMapper mapper,
            ITradesCacheService cacheService) : base(journalAccess, logger, mapper) 
        {
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
        }

        protected ActionResult ResultHandling(object? result, string logEntry, params string[] propertyNames)
        {
            if (result == null)
            {
                _logger.LogWarning(logEntry);
                return NotFound();
            }

            Type resultType = result.GetType();
            var responseObject = new Dictionary<string, object?>();

            // Check if result is a tuple (Tuples store values in fields)
            if (resultType.Name.StartsWith("ValueTuple"))
            {
                var fields = resultType.GetFields();
                for (int i = 0; i < fields.Length; i++)
                {
                    var value = fields[i].GetValue(result);
                    responseObject[propertyNames.Length > i ? propertyNames[i] : $"item{i + 1}"] = value;
                }
            }
            else if (propertyNames.Length > 0) // If result is a class and propertyNames exist
            {
                responseObject[propertyNames[0]] = result; // Pair with the first property name
            }

            // Return dictionary if propertyNames were provided, otherwise return the object
            return responseObject.Count > 0 ? Ok(responseObject) : Ok(result);
        }
    }
}
