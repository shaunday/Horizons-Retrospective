using AutoMapper;
using HsR.Journal.DataContext;
using HsR.Web.API.Controllers;
using HsR.Web.API.Services;
using HsR.Web.Services.Models.Journal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HsR.Web.API.Controllers.Journal
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [Authorize]
    public class JournalControllerBase : HsRControllerBase
    {
        private protected readonly string NEW_CELL_DATA = "updatedCellData";
        private protected readonly string NEW_ELEMENT_DATA = "updatedElement";
        private protected readonly string NEW_STATES_WRAPPER = "updatedStates";

        private protected readonly IJournalRepositoryWrapper _journalAccess;
        private protected readonly IMapper _mapper;
        private protected readonly ITradesCacheService _cacheService;

        public JournalControllerBase(
            IJournalRepositoryWrapper journalAccess, 
            ILogger<JournalControllerBase> logger, 
            IMapper mapper,
            ITradesCacheService cacheService) : base(logger) 
        {
            _journalAccess = journalAccess ?? throw new ArgumentNullException(nameof(journalAccess));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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

        protected Guid GetUserIdFromClaims()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "sub" || c.Type == "userId" || c.Type.EndsWith("nameidentifier"));
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
                throw new UnauthorizedAccessException("User ID not found in JWT claims.");
            return userId;
        }
    }
}
