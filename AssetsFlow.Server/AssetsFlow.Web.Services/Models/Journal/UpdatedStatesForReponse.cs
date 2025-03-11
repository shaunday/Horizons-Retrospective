using HsR.Journal.Entities;
using HsR.Journal.Entities.TradeJournal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AssetsFlowWeb.Services.Models.Journal
{
    public class UpdatedStatesForReponse
    {
        //element
        [JsonIgnore]
        public DateTime? elementsTimeStamp { get; set; }
        public string? FormattedTimeStamp => elementsTimeStamp?.ToString("yyyy-MM-dd HH:mm");


        //composite
        TradeSummary? newSummary { get; set; } = null;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TradeStatus newTradeStatus { get; set; }

        [JsonIgnore]
        public DateTime? CompositeOpenedAt { get; set; }
        public string? FormattedOpenedAt => CompositeOpenedAt?.ToString("yyyy-MM-dd HH:mm");

        [JsonIgnore]
        public DateTime? CompositeClosedAt { get; set; }
        public string? FormattedClosedAt => CompositeClosedAt?.ToString("yyyy-MM-dd HH:mm");
    }
}
