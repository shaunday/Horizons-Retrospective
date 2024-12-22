using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using JTA.Journal.Entities;
using JTA.Web.Services.Models.Journal;
using JTA.Journal.DataContext;

namespace JTA.Web.API.Controllers.Journal
{
    [Route("api/v{version:apiVersion}/journal/components")]
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
        public async Task<ActionResult<(DataElementModel newEntry, TradeElementModel? summary)>> UpdateDataComponent(string componentId, string newContent, string changeNote)
        {
            (DataElement updatedComponent, TradeElement? summary) = await _journalAccess.UpdateCellContentAsync(componentId, newContent, changeNote);

            (DataElementModel, TradeElementModel?) resAsModel = (_mapper.Map<DataElementModel>(updatedComponent), _mapper.Map<TradeElementModel>(summary));

            return ResultHandling(resAsModel, $"Could not update component: {componentId}");
        }

    }
}
