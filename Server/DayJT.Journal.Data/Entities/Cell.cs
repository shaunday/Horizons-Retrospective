using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DayJT.Journal.Data
{
    public class Cell
    {
        #region Props part a

        [Key]
        public int Id { get; private set; } 

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public ComponentType ComponentType { get; set; }

        public ValueRelevance CostRelevance { get; set; } = ValueRelevance.None;

        public ValueRelevance PriceRelevance { get; set; } = ValueRelevance.None;

        public bool IsRelevantForOverview { get; set; } = false;

        #endregion

        #region Ctors
        public Cell() { }

        public Cell(string title, ComponentType componentType)
        {
            Title = title;
            ComponentType = componentType;
        }
        #endregion

        [Required]
        public ContentRecord ContentWrapper { get; set; } = null!;

        public string Content
        {
            get { return ContentWrapper.Content; }
            set
            {
                ContentWrapper = new ContentRecord
                {
                    Content = value,
                    CellRef = this,
                    CellFK = Id
                };
            }
        }

        [Required]
        public ICollection<ContentRecord> History { get; set; } = new List<ContentRecord>();

        public void SetFollowupContent(string newContent, string changeNote)
        {
            History.Add(ContentWrapper);
            ContentWrapper = new ContentRecord() { Content = newContent, ChangeNote = changeNote, CellRef = this, CellFK = this.Id };
        }

        //parent
        [Required]
        public int TradeElementFK { get; set; }

        [Required]
        public TradeElement TradeElementRef { get; set; } = null!; // Required reference navigation to principal

        public void UpdateParentReference(TradeElement refObj)
        {
            TradeElementRef = refObj;
            TradeElementFK = refObj.Id;
        }
    }

}
