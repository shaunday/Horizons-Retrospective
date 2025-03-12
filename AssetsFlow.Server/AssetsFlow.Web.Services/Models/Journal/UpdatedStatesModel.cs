using HsR.Common.Extenders;
using HsR.Journal.Entities;
using HsR.Journal.Entities.TradeJournal;
using HsR.Web.Services.Models.Journal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AssetsFlowWeb.Services.Models.Journal
{
    public class UpdatedStatesModel
    {
        //element
        [JsonIgnore]
        public DateTime? ElementTimeStamp { get; set; }
        public string? FormattedElementTimeStamp => ElementTimeStamp?.ToTimeFormattedString();

        public TradeElementModel? Summary { get; set; } = null;
        
        //composite
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TradeStatus? TradeStatus { get; set; }

        [JsonIgnore]
        public DateTime? CompositeOpenedAt { get; set; }
        public string? FormattedOpenedAt => CompositeOpenedAt?.ToTimeFormattedString();

        [JsonIgnore]
        public DateTime? CompositeClosedAt { get; set; }
        public string? FormattedClosedAt => CompositeClosedAt?.ToTimeFormattedString();
    }
}
