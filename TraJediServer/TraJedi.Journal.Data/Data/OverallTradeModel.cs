using System.ComponentModel.DataAnnotations;

namespace TraJedi.Journal.Data
{
    public class OverallTradeModel
    {
        public Guid Id { get; set; }

        public Guid OverallTradeId { get; set; }

        public List<TradeInputModel> TradeInputs { get; set; } 

    }
}
