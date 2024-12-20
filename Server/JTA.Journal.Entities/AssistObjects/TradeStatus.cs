using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTA.Journal.Entities
{
    public class TradeStatus
    {
        [Required]
        public Status Status { get; set; } = Status.AnIdea;

        [Required]
        public DateTime OpenedAt { get; set; } = DateTime.Now;

        [Required]
        public int CompositeFK { get; set; }
    }

    public enum Status
    {
        AnIdea,
        Open,
        Closed
    }
}
