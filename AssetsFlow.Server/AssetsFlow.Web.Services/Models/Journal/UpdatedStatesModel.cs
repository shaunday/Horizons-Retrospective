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
        public DateTime? ActivationTimeStamp { get; set; }

        //composite
        public TradeCompositeInfo? TradeInfo { get; set; }

        //userInfo
        public ICollection<string>? SavedSectors { get; set; }
    }
}
