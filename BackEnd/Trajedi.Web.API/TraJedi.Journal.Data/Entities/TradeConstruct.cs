using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TraJedi.Journal.Data
{
    public class TradeConstruct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public ICollection<TradeInputModel> TradeInputs { get; set; } = new List<TradeInputModel>();

        public DateTime CreatedAt { get; } = DateTime.Now;

    }
}
