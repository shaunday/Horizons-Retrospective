using HsR.Journal.Entities.TradeJournal;
using HsR.Journal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HsR.Journal.Services
{
    public class UpdatedStatesCollation
    {
        public DateTime? ActivationTimeStamp { get; set; }

        public TradeComposite? TradeInfo { get; set; }

        public ICollection<string>? SavedSectors { get; set; }
    }
}
