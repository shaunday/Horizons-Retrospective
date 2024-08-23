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
    public class ContentUpdateController : JournalControllerBase
    {
        #region Ctor

        public ContentUpdateController(JournalRepository journalAccess, ILogger<JournalControllerBase> logger, IMapper mapper) :
                                                                                                        base(journalAccess, logger, mapper)
        { }

        #endregion

        [HttpPut("{componentId}")]
        public async Task<ActionResult<(CellModel? newEntry, TradeElementModel? summary)>> UpdateComponent(string componentId, string newContent, string changeNote)
        {
            (Cell? updatedComponent, TradeElement? summary) = await _journalAccess.UpdateCellContent(componentId, newContent, changeNote);

            (CellModel?, TradeElementModel?) resAsModel = (_mapper.Map<CellModel>(updatedComponent), _mapper.Map<TradeElementModel>(summary));

            return ResultHandling(resAsModel, $"Could not update component: {componentId}");
        }

    }
}
