using System.ComponentModel.DataAnnotations;

namespace TraJedi.Journal.Data
{
    public class TradeInputModel
    {
        public Guid Id { get; set; }

        public DateTime AddedAt { get; set; } 

        public TradeInputType TradeInputType { get; set; }

        public List<InputComponentModel> TradeComponents { get; set; } 


    }
}
