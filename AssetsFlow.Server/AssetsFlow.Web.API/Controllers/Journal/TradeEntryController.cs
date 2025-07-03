using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using HsR.Journal.Entities;
using HsR.Web.Services.Models.Journal;
using HsR.Journal.DataContext;
using HsR.Journal.Entities.TradeJournal;
using AssetsFlowWeb.Services.Models.Journal;
using HsR.Journal.Services;
using HsR.Web.API.Services;

namespace HsR.Web.API.Controllers.Journal
{
    [Route("hsr-api/v{version:apiVersion}/journal/components")]
    [ApiVersion("1.0")]
    [ApiController]
    public class ContentUpdateController : JournalControllerBase
    {
        public ContentUpdateController(
            IJournalRepositoryWrapper journalAccess,
            ILogger<JournalControllerBase> logger,
            IMapper mapper,
            ITradesCacheService cacheService) : base(journalAccess, logger, mapper, cacheService)
        {
        }

        [HttpPatch("{componentId}")]
        public async Task<ActionResult<(DataElementModel newEntry, UpdatedStatesModel updatedStates)>>
                                                  UpdateDataComponent(int componentId, [FromBody] UpdateDataComponentRequest request)
        {
            if (string.IsNullOrEmpty(request.Content))
            {
                return BadRequest("Content is required.");
            }

            (DataElement updatedComponent, UpdatedStatesCollation updatedStates) = 
                                                await _journalAccess.DataElement.UpdateCellContentAsync(componentId, request.Content, request.Info);

            (DataElementModel, UpdatedStatesModel) resAsModel = (_mapper.Map<DataElementModel>(updatedComponent), _mapper.Map<UpdatedStatesModel>(updatedStates));

            // Invalidate cache for the user who owns the entry
            var userId = updatedComponent.UserId;
            if (userId != Guid.Empty)
                _cacheService.InvalidateAndReload(userId);

            return ResultHandling(resAsModel, $"Could not update component: {componentId}", [NEW_CELL_DATA, NEW_STATES_WRAPPER]);
        }

        public class UpdateDataComponentRequest
        {
            public string Content { get; set; } = "";
            public string Info { get; set; } = "";
        }
    }    
}
