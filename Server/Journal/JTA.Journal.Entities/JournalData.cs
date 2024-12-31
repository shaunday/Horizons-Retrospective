using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTA.Journal.Entities
{
    public class JournalData
    {
        public List<string>? SavedSectors { get; set; }

        public List<string>? SavedBrokers { get; set; }

    }
}
