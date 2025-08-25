using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketWatch.Entities
{
    public class SecurityModel
    {
        [Key]
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string Exchange { get; set; }
        public SecurityType SecurityType { get; set; }
        public string Currency { get; set; }
        public List<PriceBarModel> PriceBars { get; set; } = [];

        public SecurityModel(string symbol, string exchange, SecurityType securityType, string currency)
        {
            Symbol = symbol;
            Exchange = exchange;
            SecurityType = securityType;
            Currency = currency;
        }
    }
}
