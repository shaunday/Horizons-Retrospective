namespace DayJTrading.Journal.Data
{
    public class FilterInfoPair
    {
        public required string Title { get; set; }
        public required string FilterValue { get; set; }
    }

    public class TradesFilterModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<FilterInfoPair>? FilterObjects { get; set; }
    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      