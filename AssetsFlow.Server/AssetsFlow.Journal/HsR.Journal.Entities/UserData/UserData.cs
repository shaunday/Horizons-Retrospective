using System.ComponentModel.DataAnnotations;

namespace HsR.Journal.Entities
{
    public class UserData
    {
        [Key]
        public Guid UserId { get; set; }
        
        public ICollection<string>? SavedSectors { get; set; }
    }
}
