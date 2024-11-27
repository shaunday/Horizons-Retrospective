using DayJT.Journal.DataEntities.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DayJT.Journal.Data
{
    public class Cell : TradeCompositeNavDataBase
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
                ContentWrapper = new ContentRecord(this, value);
            }
        }

        [Required]
        public ICollection<ContentRecord> History { get; set; } = new List<ContentRecord>();

        public void SetFollowupContent(string newContent, string changeNote)
        {
            History.Add(ContentWrapper);
            ContentWrapper = new ContentRecord(parentCell:this, content: newContent) { ChangeNote = changeNote };
        }

        //parent
        [Required]
        public int TradeElementFK { get; set; }

        [Required]
        public TradeElement TradeElementRef { get; set; } = null!; 

        public void UpdateParentReference(TradeElement refObj)
        {
            TradeElementRef = refObj;
        }
    }

}
