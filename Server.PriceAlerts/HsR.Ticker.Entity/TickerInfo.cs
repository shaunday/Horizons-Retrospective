using System.ComponentModel.DataAnnotations;

namespace HsR.Ticker.Entity
{
    public class TickerInfo
    {
        [Key]
        public int Id { get; private set; }

        [Required]
        public string Symbol { get; set; } = null!;

        [Required]
        public string Name { get; set; } = null!;
    }
}
