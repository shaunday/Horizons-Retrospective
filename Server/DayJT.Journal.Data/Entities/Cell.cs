using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DayJT.Journal.Data
{
    public class Cell
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
        public CellContent ContentWrapper { get; set; } = new CellContent();

        public string Content
        {
            get { return ContentWrapper.Content; }
            set { ContentWrapper.Content = value; }
        }
        #endregion

        #region flags

        [MaxLength(50)]
        public ValueRelevance CostRelevance { get; set; } = ValueRelevance.None;

        [MaxLength(50)]
        public ValueRelevance PriceRelevance { get; set; } = ValueRelevance.None;

        public bool IsRelevantForOverview { get; set; } = false;

        #endregion

        public ICollection<CellContent> History { get; set; } = new List<CellContent>();


        public Cell() { }

        public Cell(string title)
        {
            Title = title;
        }

        //parent
        public Guid TradeComponentRefId { get; set; }

        public TradeComponent TradeComponentRef { get; set; }

    }

}
