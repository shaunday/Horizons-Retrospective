using Asp.Versioning;
using HsR.Web.Services.Models.Journal;
using AutoMapper;
using HsR.Journal.DataContext;
using HsR.Journal.Repository;
using HsR.Journal.Services;
using HsR.Web.API.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using HsR.Journal.Entities;
using Microsoft.AspNetCore.Authorization;

namespace HsR.Web.API.Controllers.Journal
{
    [Route("hsr-api/v{version:apiVersion}/journal/components/{componentId}")]
    [ApiVersion("1.0")]
    [ApiController]
    public class ContentUpdateController : JournalOpsControllerBase
    {
        public ContentUpdateController(
            IJournalRepositoryWrapper journalAccess,
            ILogger<JournalOpsControllerBase> logger,
            IMapper mapper,
            TradesCacheService cacheService) : base(journalAccess, logger, mapper, cacheService)
        {
        }

        [HttpPatch]
        public async Task<ActionResult<(DataElementModel newEntry, UpdatedStatesModel updatedStates)>>
                                                  UpdateDataComponent(int componentId, [FromBody] UpdateDataComponentRequest request)
        {
            if (string.IsNullOrEmpty(request.Content))
            {
                return BadRequest("Content is required.");
            }
            try
            {
                (DataElement updatedComponent, UpdatedStatesCollation updatedStates) =
                    await _journalAccess.DataElement.UpdateCellContentAsync(componentId, request);
                DataElementModel newEntry = _mapper.Map<DataElementModel>(updatedComponent);
                UpdatedStatesModel updatedStatesModel = _mapper.Map<UpdatedStatesModel>(updatedStates);
                _cacheService.InvalidateAndReload(updatedComponent.UserId);

                (DataElementModel, UpdatedStatesModel) resAsModel = (newEntry, updatedStatesModel);
                return ResultHandling(resAsModel, $"Could not update component: {componentId}", [NEW_CELL_DATA, NEW_STATES_WRAPPER]);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error updating data component with Id: {ComponentId}", componentId);
                return NotFound();
            }
        }

       
    }    
}
