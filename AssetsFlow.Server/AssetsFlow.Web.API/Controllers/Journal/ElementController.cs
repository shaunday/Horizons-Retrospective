
using Asp.Versioning;
using AutoMapper;
using HsR.Journal.DataContext;
using HsR.Journal.Entities;
using HsR.Web.API.Controllers.Journal;
using HsR.Web.Services.Models.Journal;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

[Route("api/v{version:apiVersion}/journal/elements/{elementId}")]
[ApiVersion("1.0")]
[ApiController]
public class TradeElementsController(IJournalRepositoryWrapper journalAccess,
        ILogger<JournalControllerBase> logger, IMapper mapper) : JournalControllerBase(journalAccess, logger, mapper)
{
    [HttpPatch("activate")]
    public async Task<ActionResult<DateTime>> ActivateTradeElment(string elementId)
    {
        var tradeElement = await _journalAccess.ActivateTradeElement(elementId);

        if (tradeElement == null)
        {
            return NotFound();
        }

        return ResultHandling((tradeElement.TimeStamp), $"Could not activate element with Id: {elementId}", [NEW_TIMESTAMP]);
    }

    [HttpDelete("{tradeInputId}")]
    public async Task<ActionResult<TradeElementModel>> DeleteInterimTradeInput(string elementId)
    {
        var summary = await _journalAccess.RemoveInterimPositionAsync(elementId);
        if (summary == null)
        {
            return NotFound();
        }

        TradeElementModel resAsModel = _mapper.Map<TradeElementModel>(summary);

        return ResultHandling(resAsModel, $"Could not delete element with Id: {elementId}", [NEW_SUMMARY]);
    }
}
    