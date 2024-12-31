using JTA.Journal.Entities;
using System.ComponentModel.DataAnnotations;

namespace JTA.Web.Services.Models.Journal
{
    public class TradeElementModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public TradeActionType TradeActionType { get; set; }

        public List<DataElementModel> Entries { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        [Required]
        public int TradeCompositeFK { get; set; }
    }
}
