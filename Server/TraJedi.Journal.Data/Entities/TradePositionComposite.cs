using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TraJedi.Journal.Data
{
    public class TradePositionComposite
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public ICollection<TradeInfoSingleLine> TradeComponents { get; set; } = new List<TradeInfoSingleLine>();

        public DateTime CreatedAt { get; } = DateTime.Now;

    }
}
