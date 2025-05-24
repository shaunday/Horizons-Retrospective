
using Asp.Versioning;
using AssetsFlowWeb.Services.Models.Journal;
using AutoMapper;
using HsR.Journal.DataContext;
using HsR.Journal.Entities;
using HsR.Web.API.Controllers.Journal;
using HsR.Web.Services.Models.Journal;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

[Route("hsr-api/v{version:apiVersion}/journal/elements/{elementId}")]
[ApiVersion("1.0")]
[ApiController]
public class TradeElementsController(IJournalRepositoryWrapper journalAccess,
        ILogger<JournalControllerBase> logger, IMapper mapper) : JournalControllerBase(journalAccess, logger, mapper)
{
    [HttpDelete]
    public async Task<ActionResult<UpdatedStatesModel>> DeleteInterimTradeInput(string elementId)
    {
        var updatedStates = await _journalAccess.TradeElement.RemoveInterimPositionAsync(elementId);
        if (updatedStates == null)
        {
            return NotFound();
        }

        UpdatedStatesModel resAsModel = _mapper.Map<UpdatedStatesModel>(updatedStates);

        return ResultHandling(updatedStates, $"Could not delete element with Id: {elementId}", [NEW_STATES_WRAPPER]);
    }

    [HttpPatch]
    public async Task<ActionResult<UpdatedStatesModel>> ReTimestampTradeInput(string elementId, [FromQuery] string newTime)
    {
        var updatedStates = await _journalAccess.TradeElement.UpdateActivationTimeAsync(elementId, newTime);
        if (updatedStates == null)
        {
            return NotFound();
        }

        UpdatedStatesModel resAsModel = _mapper.Map<UpdatedStatesModel>(updatedStates);

        return ResultHandling(updatedStates, $"Could not reactivate element with Id: {elementId}", [NEW_STATES_WRAPPER]);
    }
}
    