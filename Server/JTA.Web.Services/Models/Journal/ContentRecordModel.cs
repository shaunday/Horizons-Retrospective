using DayJT.Journal.DataEntities.Entities;
using System.ComponentModel.DataAnnotations;

namespace DayJTrading.Web.Services.Models.Journal
{
    public class ContentRecordModel
    {
        [Required]
        public string Content { get; set; } = string.Empty;

        public string? ChangeNote { get; set; } 

        public DateTime CreatedAt { get; set; }

        [Required]
        public int DataElementFK { get; set; }

    }
}
