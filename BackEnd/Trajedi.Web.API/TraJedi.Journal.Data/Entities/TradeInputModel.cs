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

        public List<InputComponentModel> TradeInputComponents { get; set; } = new List<InputComponentModel>();


        //parent
        [ForeignKey("TradeConstructId")]
        public TradeConstruct TradeConstruct { get; set; } = null!;

        public Guid TradeConstructId { get; set; }

    }
}
