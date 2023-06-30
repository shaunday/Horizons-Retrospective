using TraJedi.Journal.Data;

namespace TraJedi.Web.API.Models
{
    public class TradeCompositeModel
    {
        public Guid Id { get; set; }
        public ICollection<TradeComponentModel> TradeComponents { get; set; }
        public DateTime CreatedAt { get; set;  }
    }
}
