using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TraJedi.Journal.Data
{
    public class InputComponentModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(50)]
        public ComponentType ComponentType { get; set; }

        #region Content
        public ContentModel ContentWrapper { get; set; } = new ContentModel();

        public string Content
        {
            get { return ContentWrapper.Content; }
            set { ContentWrapper.Content = value; }
        }
        #endregion

        #region flags

        public ValueRelevance CostRelevance { get; set; } = ValueRelevance.None;

        public ValueRelevance PriceValueRelevance { get; set; } = ValueRelevance.None;

        public bool AttachedToggle { get; set; } = false;

        public bool IsRelevantForOverview { get; set; } = false;

        #endregion

        public ICollection<ContentModel> History { get; set; } = new List<ContentModel>();


        public InputComponentModel(string title)
        {
            Title = title;
        }

        //parent
        [ForeignKey("TradeInputModelId")]
        public TradeInputModel TradeInputModel { get; set; } = null!;

        public Guid TradeInputModelId { get; set; }
    }

}
