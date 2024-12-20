using JTA.Journal.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTA.Web.Services.Models.Journal
{
    public class TradeStatusModel
    {
        [Required]
        public Status Status { get; set; } 

        [Required]
        public DateTime OpenedAt { get; set; } 

        [Required]
        public int CompositeFK { get; set; }
    }
}
