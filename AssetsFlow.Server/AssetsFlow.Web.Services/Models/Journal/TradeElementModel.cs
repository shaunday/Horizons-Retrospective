using HsR.Journal.Entities;
using System.ComponentModel.DataAnnotations;

namespace HsR.Web.Services.Models.Journal
{
    public class TradeElementModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public TradeActionType TradeActionType { get; set; }

        [Required]
        public List<DataElementModel> Entries { get; set; } = null!;

        public DateTime TimeStamp { get; set; }

        [Required]
        public bool IsActive { get; set; } = false;

        [Required]
        public int TradeCompositeFK { get; set; }
    }
}
