using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TraJedi.Journal.Data
{
    public class ContentModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Content { get; set; } = string.Empty;

        public DateTime AddedAt { get; } = DateTime.Now;

        //parent
        [ForeignKey("InputComponentModelId")]
        public InputComponentModel? InputComponentModel { get; set; }

        public Guid InputComponentModelId { get; set; }
    }
}
