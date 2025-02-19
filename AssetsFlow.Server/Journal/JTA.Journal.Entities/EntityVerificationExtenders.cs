using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HsR.Journal.Entities
{
    public static class EntityVerificationExtenders
    {
        //composite
        public static bool IsTadeActive(this TradeComposite trade)
        {
            return trade.TradeElements.Any(ele => ele.AllowActivation() && ele.TradeActionType == TradeActionType.Add);
        }


        //trade elements
        public static bool AllowActivation(this TradeElement element)
        {
            return !element.Entries.Any(e => e.IsMustHave && string.IsNullOrEmpty(e.Content));
        }


        //data elements
        public static bool IsCostRelevant(this DataElement dataElement)
        {
            return dataElement.UnitPriceRelevance != null || dataElement.TotalCostRelevance != null;

        }
    }
}
