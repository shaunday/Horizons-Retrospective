using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TraJedi.Journal.Data
{
    public class TradeInputModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public TradeModel ParentTrade { get; set; }

        public DateTime AddedAt { get; set; } 

        public TradeInputType TradeInputType { get; set; }

        public ICollection<InputComponentModel> TradeComponents { get; set; } = new List<InputComponentModel>();

    }
}
