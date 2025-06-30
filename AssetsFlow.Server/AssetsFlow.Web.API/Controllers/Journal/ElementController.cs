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
    public async Task<ActionResult<UpdatedStatesModel>> DeleteInterimTradeInput(Guid userId, string elementId)
    {
        var updatedStates = await _journalAccess.TradeElement.RemoveInterimPositionAsync(userId, elementId);
        if (updatedStates == null)
        {
            return NotFound();
        }

        UpdatedStatesModel resAsModel = _mapper.Map<UpdatedStatesModel>(updatedStates);

        _cacheService.InvalidateAndReload(userId);

        return ResultHandling(resAsModel, $"Could not delete element with Id: {elementId}", [NEW_STATES_WRAPPER]);
    }

    [HttpPatch]
    public async Task<ActionResult<UpdatedStatesModel>> ReTimestampTradeInput(Guid userId, string elementId, string newTime)
    {
        var updatedStates = await _journalAccess.TradeElement.UpdateActivationTimeAsync(userId, elementId, newTime);
        if (updatedStates == null)
        {
            return NotFound();
        }

        UpdatedStatesModel resAsModel = _mapper.Map<UpdatedStatesModel>(updatedStates);

        _cacheService.InvalidateAndReload(userId);

        return ResultHandling(resAsModel, $"Could not reactivate element with Id: {elementId}", [NEW_STATES_WRAPPER]);
    }
}
    