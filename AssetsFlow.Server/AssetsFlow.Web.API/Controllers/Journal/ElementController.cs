using Asp.Versioning;
using AssetsFlowWeb.Services.Models.Journal;
using AutoMapper;
using HsR.Journal.DataContext;
using HsR.Web.API.Controllers.Journal;
using HsR.Web.API.Services;
using Microsoft.AspNetCore.Mvc;

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
        var updatedStates = await _journalAccess.TradeElement.RemoveInterimPositionAsync(elementId);
        if (updatedStates == null)
        {
            return NotFound();
        }
        _cacheService.InvalidateAndReload(updatedStates.TradeInfo!.UserId);

        UpdatedStatesModel resAsModel = _mapper.Map<UpdatedStatesModel>(updatedStates);

        return ResultHandling(resAsModel, $"Could not delete element with Id: {elementId}", [NEW_STATES_WRAPPER]);
    }

    [HttpPatch]
    public async Task<ActionResult<UpdatedStatesModel>> ReTimestampTradeInput(int elementId, string newTime)
    {
        var updatedStates = await _journalAccess.TradeElement.UpdateActivationTimeAsync(elementId, newTime);
        if (updatedStates == null)
        {
            return NotFound();
        }

        UpdatedStatesModel resAsModel = _mapper.Map<UpdatedStatesModel>(updatedStates);

        // Invalidate cache for the user who owns the element
        var userId = updatedStates.TradeInfo?.UserId ?? Guid.Empty;
        if (userId != Guid.Empty)
            _cacheService.InvalidateAndReload(userId);

        return ResultHandling(resAsModel, $"Could not reactivate element with Id: {elementId}", [NEW_STATES_WRAPPER]);
    }
}
    