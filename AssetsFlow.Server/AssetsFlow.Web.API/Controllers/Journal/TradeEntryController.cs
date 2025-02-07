using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using HsR.Journal.Entities;
using HsR.Web.Services.Models.Journal;
using HsR.Journal.DataContext;

namespace HsR.Web.API.Controllers.Journal
{
    [Route("api/v{version:apiVersion}/journal/components")]
    [ApiVersion("1.0")]
    [ApiController]
    public class ContentUpdateController : JournalControllerBase
    {
        #region Ctor

        public ContentUpdateController(JournalRepositoryWrapper journalAccess, ILogger<JournalControllerBase> logger, IMapper mapper) :
                                                                                                        base(journalAccess, logger, mapper)
        { }

        #endregion

        [HttpPut("{componentId}")]
        public async Task<ActionResult<(DataElementModel newEntry, TradeElementModel? summary)>>
                                                UpdateDataComponent(string componentId, [FromBody] UpdateDataComponentRequest request)
        {
            if (string.IsNullOrEmpty(request.Content))
            {
                return BadRequest("Content is required.");
            }

            (DataElement updatedComponent, TradeElement? summary) = await _journalAccess.UpdateCellContentAsync(componentId, request.Content, request.Info);

            (DataElementModel, TradeElementModel?) resAsModel = (_mapper.Map<DataElementModel>(updatedComponent), _mapper.Map<TradeElementModel>(summary));

            return ResultHandling(resAsModel, $"Could not update component: {componentId}");
        }

        public class UpdateDataComponentRequest
        {
            public string Content { get; set; } = "";
            public string Info { get; set; } = "";
        }

    }    
}
