
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
    public async Task<ActionResult<bool>> ActivateTradeElment(string elementId)
    {
        var tradeElement = await _journalAccess.ActivateTradeElement(elementId);

        if (tradeElement == null)
        {
            return NotFound();
        }

        return ResultHandling((tradeElement.IsActive), $"Could not activate element on : {elementId}", [NEW_ELEMENT_DATA]);
    }
}
    