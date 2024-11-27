using DayJT.Journal.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayJT.Journal.DataEntities.Entities
{
    public class TradeCompositeNavDataBase
    {
        [Required]
        public int TradeCompositeFK { get; set; }

        [Required]
        public TradeComposite TradeCompositeRef { get; set; } = null!;
    }
}
