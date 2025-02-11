using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HsR.Journal.Entities
{
    public static class EntityVerificationExtenders
    {
        //trade elements
        public static bool AllowActivation(this TradeAction element)
        {
            return !element.Entries.Select(e => e.IsMustHave && string.IsNullOrEmpty(e.Content)).Any();
        }


        //data elements
        public static bool IsCostRelevant(this DataElement dataElement)
        {
            return dataElement.UnitPriceRelevance != null || dataElement.TotalCostRelevance != null;

        }
    }
}
