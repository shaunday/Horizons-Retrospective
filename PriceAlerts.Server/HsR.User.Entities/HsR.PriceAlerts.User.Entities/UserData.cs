namespace HsR.PriceAlerts.User.Entities
{
    public class UserData
    {
        public int Id { get; set; }

        //todo configure
        public Dictionary<int, DateTime> AlertsLastPushPerTicker { get; set; }
    }
}
