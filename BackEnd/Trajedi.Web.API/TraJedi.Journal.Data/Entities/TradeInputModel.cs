using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TraJedi.Journal.Data
{
    public class TradeInputModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public TradeInputType TradeInputType { get; set; }

        public DateTime AddedAt { get; set; } 

        public ICollection<InputComponentModel> TradeComponents { get; set; } = new List<InputComponentModel>();


        //parent
        [ForeignKey("TradeModelId")]
        public TradeModel? TradeModel { get; set; }

        public Guid TradeModelId { get; set; }

    }
}
