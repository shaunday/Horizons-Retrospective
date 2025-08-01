using Asp.Versioning;
using HsR.Web.Services.Models.Journal;
using AutoMapper;
using HsR.Journal.DataContext;
using HsR.Journal.Services;
using HsR.Web.API.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using HsR.Web.API.Controllers.Journal;
using Microsoft.AspNetCore.Authorization;

[Route("hsr-api/v{version:apiVersion}/journal/elements/{elementId}")]
[ApiVersion("1.0")]
[ApiController]
public class TradeElementsController(
    IJournalRepositoryWrapper journalAccess,
    ILogger<JournalControllerBase> logger,
    IMapper mapper,
    ITradesCacheService cacheService) : JournalControllerBase(journalAccess, logger, mapper, cacheService)
{
    [HttpDelete]
    public async Task<ActionResult<UpdatedStatesModel>> DeleteInterimTradeInput(int elementId)
    {
        try
        {
            TradeComposite updatedTrade = await _journalAccess.TradeElement.RemoveInterimPositionAsync(elementId);
            _cacheService.InvalidateAndReload(updatedTrade.UserId);
            UpdatedStatesModel resAsModel = _mapper.Map<UpdatedStatesModel>(new UpdatedStatesCollation() { TradeInfo = updatedTrade });
            return ResultHandling(resAsModel, $"Could not delete element with Id: {elementId}", [NEW_STATES_WRAPPER]);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error deleting interim trade input with Id: {ElementId}", elementId);
            var (status, msg) = ExceptionMappingService.MapToStatusCode(ex);
            return StatusCode(status, msg);
        }
    }

    [HttpPatch]
    public async Task<ActionResult<UpdatedStatesModel>> ReTimestampTradeInput(int elementId, string newTime)
    {
        try
        {
            var updatedStates = await _journalAccess.TradeElement.UpdateActivationTimeAsync(elementId, newTime);
            UpdatedStatesModel resAsModel = _mapper.Map<UpdatedStatesModel>(updatedStates);
            _cacheService.InvalidateAndReload(updatedStates.TradeInfo?.UserId ?? Guid.Empty);
            return ResultHandling(resAsModel, $"Could not re-timestamp element with Id: {elementId}", [NEW_STATES_WRAPPER]);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error re-timestamping trade input with Id: {ElementId}", elementId);
            var (status, msg) = ExceptionMappingService.MapToStatusCode(ex);
            return StatusCode(status, msg);
        }
    }
}
    