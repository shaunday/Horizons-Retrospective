using DayJT.Journal.Data;
using System.ComponentModel.DataAnnotations;

namespace DayJT.Journal.DataEntities.Entities
{
    public class DataElement
    {
        #region Props 

        [Key]
        public int Id { get; private set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public ComponentType ComponentType { get; set; }

        public ValueRelevance? CostRelevance { get; set; } 

        public ValueRelevance? PriceRelevance { get; set; } 

        [Required]
        public bool IsRelevantForOverview { get; set; } = false;

        #endregion

        #region Ctors
        public DataElement() { }

        public DataElement(string title, ComponentType componentType)
        {
            Title = title;
            ComponentType = componentType;
        }
        #endregion

        [Required]
        public ContentRecord ContentWrapper { get; set; } = null!;

        public string Content
        {
            get { return ContentWrapper.ContentValue; }
            set
            {
                ContentWrapper = new ContentRecord(value);
            }
        }

        public ICollection<ContentRecord>? History { get; set; }

        public void SetFollowupContent(string newContent, string changeNote)
        {
            History ??= new List<ContentRecord>();
            History.Add(ContentWrapper);
            ContentWrapper = new ContentRecord(content: newContent) { ChangeNote = changeNote };
        }


        public void UpdateParentRefs(TradeElement refObj)
        {
            TradeElementRef = refObj;
            CompositeRef = refObj.CompositeRef;
        }

        #region Refs, FKs
        [Required]
        public int TradeElementFK { get; set; }

        [Required]
        public TradeElement TradeElementRef { get; set; } = null!;

        [Required]
        public int CompositeFK { get; set; }

        [Required]
        public TradeComposite CompositeRef { get; set; } = null!; 
        #endregion
    }

}
