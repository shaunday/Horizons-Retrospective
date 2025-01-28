namespace HsR.Journal.Entities
{
    public class UserData
    {
        public ICollection<string>? SavedSectors { get; set; }

        public bool AllowPublicView { get; set; }

        public bool AnonymizedPublicView { get; set; }

    }
}
