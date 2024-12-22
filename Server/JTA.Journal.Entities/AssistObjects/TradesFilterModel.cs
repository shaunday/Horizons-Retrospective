namespace JTA.Journal.Entities
{
    public class FilterInfoPair
    {
        public required string Title { get; set; }
        public required string FilterValue { get; set; }
    }

    public class TradesFilterModel
    {
        public DateTime? OpenLowerLimit { get; set; }
        public DateTime? OpenUpperLimit { get; set; }

        public DateTime? CloseLowerLimit { get; set; }
        public DateTime? CloseUpperLimit { get; set; }
        public List<FilterInfoPair>? FilterObjects { get; set; }
    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      