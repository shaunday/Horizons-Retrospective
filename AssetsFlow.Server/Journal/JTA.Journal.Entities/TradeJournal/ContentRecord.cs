using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HsR.Journal.Entities
{
    public class ContentRecord
    {
        // Private parameterless constructor for EF Core
        private ContentRecord() { }

        [SetsRequiredMembers]
        public ContentRecord(string content)
        {
            ContentValue = content;
        }

        [Required]
        public required string ContentValue { get; set; } = string.Empty;

        public string? ChangeNote { get; set; }

        [Required]
        public DateTime TimeStamp { get; } = DateTime.Now;

        [Required]
        public int DataElementFK { get; set; }  // FK property to the owning DataCell

        public override string ToString() => $"Content={ContentValue}";
    }
}
