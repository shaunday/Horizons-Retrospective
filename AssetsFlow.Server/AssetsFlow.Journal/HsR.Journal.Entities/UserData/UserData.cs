namespace HsR.Journal.Entities
{
    public class UserData
    {
        public int Id { get; set; } = 1; // Fixed ID, ensures only one row
        public ICollection<string>? SavedSectors { get; set; } = [];
    }
}
