using HsR.Journal.Entities.TradeJournal;
using System.ComponentModel.DataAnnotations;

namespace HsR.Web.Services.Models.Journal
{
    public class ContentRecordModel
    {
        [Required]
        public string Content { get; set; } = string.Empty;

        public string? ChangeNote { get; set; } 

        public DateTime TimeStamp { get; set; }

        [Required]
        public int DataElementFK { get; set; }

    }
}
