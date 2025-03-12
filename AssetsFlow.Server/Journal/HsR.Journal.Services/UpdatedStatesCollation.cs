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
        public DateTime? ElementTimeStamp { get; set; }

        public TradeSummary? Summary { get; set; } = null;

        public TradeStatus? TradeStatus { get; set; }
        public DateTime? CompositeOpenedAt { get; set; }
        public DateTime? CompositeClosedAt { get; set; }

        public ICollection<string>? SavedSectors { get; set; }
    }
}
