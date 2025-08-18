using HsR.Common.Extenders;
using HsR.Journal.Entities.Factory.Models;
using HsR.Journal.Entities.Factory.Services;
using HsR.Journal.Entities.TradeJournal;
using HsR.Journal.TradeAnalytics;

namespace HsR.Journal.Entities.Factory
{
    public static partial class TradeElementsFactory
    {
        #region Origin
        private static List<DataElement> GetOriginEntries(InterimTradeElement elementRef)
        {
            var template = TradeElementTemplateLoader.GetTemplate(TradeActionType.Origin);
            return CreateEntries(CreateDataElementsFromTemplate(template), elementRef);
        }
        #endregion

        #region Interim elements
        private static List<DataElement> GetAddPositionEntries(InterimTradeElement elementRef)
        {
            var template = TradeElementTemplateLoader.GetTemplate(TradeActionType.Add);
            return CreateEntries(CreateDataElementsFromTemplate(template), elementRef);
        }

        private static List<DataElement> GetEvalutationEntries(InterimTradeElement elementRef)
        {
            var template = TradeElementTemplateLoader.GetTemplate(TradeActionType.Evaluation);
            return CreateEntries(CreateDataElementsFromTemplate(template), elementRef);
        }

        private static List<DataElement> GetFirstPositionEntries(InterimTradeElement elementRef)
        {
            var template = TradeElementTemplateLoader.GetTemplate(TradeActionType.Add);
            return CreateEntries(CreateDataElementsFromTemplate(template, isFirstPosition: true), elementRef);
        }

        private static List<DataElement> GetReducePositionEntries(InterimTradeElement elementRef)
        {
            var template = TradeElementTemplateLoader.GetTemplate(TradeActionType.Reduce);
            return CreateEntries(CreateDataElementsFromTemplate(template), elementRef);
        }
        #endregion

        #region Summary and Closure
        private static List<DataElement> GetSummaryComponents(TradeSummary elementRef, TradeAnalyticsSummary analytics)
        {
            var template = TradeElementTemplateLoader.GetTemplate(TradeActionType.Summary);
            return CreateEntries(CreateSummaryDataElementsFromTemplate(template, analytics), elementRef);
        }

        private static List<DataElement> GetTradeClosureComponents(TradeSummary elementRef, TradeAnalyticsSummary analytics)
        {
            var template = TradeElementTemplateLoader.GetTemplate(TradeActionType.Closure);
            return CreateEntries(CreateSummaryDataElementsFromTemplate(template, analytics), elementRef);
        }
        #endregion

        private static DataElement CreateEntry(DataElement overview, TradeElement elementRef)
        {
            overview.UpdateParentRefs(elementRef);
            return overview;
        }

        private static List<DataElement> CreateEntries(IEnumerable<DataElement> cellConfigs, TradeElement elementRef)
        {
            return cellConfigs.Select(config => CreateEntry(config, elementRef)).ToList();
        }

        private static List<DataElement> CreateDataElementsFromTemplate(TradeElementTemplate template, bool isFirstPosition = false)
        {
            return CreateDataElementsInternal(template, isFirstPosition, analytics: null);
        }

        private static List<DataElement> CreateSummaryDataElementsFromTemplate(TradeElementTemplate template, TradeAnalyticsSummary analytics)
        {
            return CreateDataElementsInternal(template, isFirstPosition: false, analytics);
        }

        private static List<DataElement> CreateDataElementsInternal(TradeElementTemplate template, bool isFirstPosition, TradeAnalyticsSummary? analytics)
        {
            var elements = new List<DataElement>();

            foreach (var el in template.Elements)
            {
                if (el.FirstPositionOnly == true && !isFirstPosition)
                    continue;

                string content = analytics is TradeAnalyticsSummary summary
                    ? el.Title switch
                     {
                         "Average Entry" => summary.AverageEntryPrice.ToF2String(),
                         "Average Close" => summary.AverageExitPrice.ToF2String(),
                         "Total Amount" => summary.NetAmount.ToF2String(),
                         "Net Result" => summary.Profit.ToF2String(),
                         "W/L" => summary.IsWin ? "W" : "L",
                         _ => ""
                     }
                     : "";

                var dataElement = new DataElement(el.Title, el.ComponentType, content)
                {
                    IsRelevantForLocalOverview = el.IsRelevantForLocalOverview ?? false,
                    IsRelevantForTradeOverview = el.IsRelevantForTradeOverview ?? false,
                    Restrictions = el.Restrictions,
                    UnitPriceRelevance = el.UnitPriceRelevance,
                    TotalCostRelevance = el.TotalCostRelevance
                };

                elements.Add(dataElement);
            }

            return elements;
        }
    }
}

