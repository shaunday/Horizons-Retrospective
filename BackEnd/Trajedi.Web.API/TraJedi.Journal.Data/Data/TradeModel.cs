namespace TraJedi.Journal.Data
{
    public class TradeModel
    {
        public Guid Id { get; set; }

        public List<TradeInputModel> TradeInputs { get; set; } = new List<TradeInputModel>();   

    }
}
