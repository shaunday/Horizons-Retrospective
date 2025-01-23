using System.ComponentModel.DataAnnotations;

namespace HsR.Journal.Entities
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

        public ValueRelevance? UnitPriceRelevance { get; set; } 

        public ValueRelevance? TotalCostRelevance { get; set; }

        public bool IsCostRelevant => UnitPriceRelevance != null || TotalCostRelevance != null;

        [Required]
        public bool IsRelevantForOverview { get; set; } = false;

        public ICollection<ContentRecord>? History { get; set; }

        #endregion

        #region Ctors
        private DataElement() { } //for ef

        public DataElement(string title, ComponentType componentType, string content = "")
        {
            Title = title;
            ComponentType = componentType;

            if (!string.IsNullOrEmpty(content))
            {
                ContentWrapper = new ContentRecord(content);
            }
        }
        #endregion

        [Required]
        public ContentRecord? ContentWrapper { get; set; }

        public string? Content => ContentWrapper?.ContentValue ;

        public void SetFollowupContent(string newContent, string changeNote)
        {
            if (ContentWrapper != null)
            {
                History ??= new List<ContentRecord>();
                History.Add(ContentWrapper);
            }
           
            ContentWrapper = new ContentRecord(content: newContent) { ChangeNote = changeNote };
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

        public void UpdateParentRefs(TradeElement refObj)
        {
            TradeElementRef = refObj;
            CompositeRef = refObj.CompositeRef;
        }
        #endregion
    }

}
