using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DayJT.Journal.Data
{
    public class Cell
    {
        #region Props part a

        [Key]
        public Guid Id { get; private set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public ComponentType ComponentType { get; set; }

        public BasicCellType CellType { get; private set; }


        [MaxLength(50)]
        public ValueRelevance CostRelevance { get; set; } = ValueRelevance.None;

        [MaxLength(50)]
        public ValueRelevance PriceRelevance { get; set; } = ValueRelevance.None;

        public bool IsRelevantForOverview { get; set; } = false;

        #endregion

        #region Ctors
        public Cell() { }

        public Cell(BasicCellType cellType, string title)
        {
            Title = title;
            CellType = cellType;
        }
        #endregion

        public CellContent ContentWrapper { get; set; }

        public string Content
        {
            get { return ContentWrapper.Content; }
            set
            {
                ContentWrapper = new CellContent
                {
                    Content = value,
                    CellRef = this,
                    CellRefId = Id
                };
            }
        }

        public ICollection<CellContent> History { get; set; } = new List<CellContent>();

        public void SetFollowupContent(string newContent, string changeNote)
        {
            History.Add(ContentWrapper);
            ContentWrapper = new CellContent() { Content = newContent, ChangeNote = changeNote, CellRef = this, CellRefId = this.Id };
        }

        //parent
        public Guid TradeElementRefId { get; set; }

        public TradeElement TradeElementRef { get; set; } = null!; // Required reference navigation to principal

        public void UpdateParentReference(TradeElement refObj)
        {
            TradeElementRef = refObj;
            TradeElementRefId = refObj.Id;
        }
    }

}
