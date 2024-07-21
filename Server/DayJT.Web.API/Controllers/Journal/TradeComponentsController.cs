using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using DayJT.Journal.Data;
using DayJT.Web.API.Models;
using DayJT.Journal.DataContext.Services;

namespace DayJT.Web.API.Controllers.Journal
{
    [Route("api/v{version:apiVersion}/journal/trades/components")]
    [ApiVersion("1.0")]
    [ApiController]
    public class TradeComponentsController : JournalControllerBase
    {
        #region Ctor

        public TradeComponentsController(JournalRepository journalAccess, ILogger<JournalControllerBase> logger, IMapper mapper) :
                                                                                                        base(journalAccess, logger, mapper)
        { }

        #endregion

        [HttpPut("{componentId}")]
        public async Task<ActionResult> UpdateComponent(string componentId, string newContent, string changeNote)
        {
            (Cell? updatedComponent, TradeComponent? summary) = await _journalAccess.UpdateCellContent(componentId, newContent, changeNote);

            (CellModel?, TradeComponentModel?) resAsModel = (_mapper.Map<CellModel>(updatedComponent), _mapper.Map<TradeComponentModel>(summary));

            return ResultHandling(resAsModel, $"Could not update component: {componentId}");
        }

    }
}
