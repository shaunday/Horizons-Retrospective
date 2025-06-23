//general
export const HasMissingContent = 'isAnyContentMissing'; 

//trade
export const TRADE_SUMMARY_STRING = 'summary';
export const TRADE_ELEMENTS_STRING = 'tradeElements'
export const TRADE_ENTRIES_STRING = 'entries'
export const TRADE_STATUS = 'status'; 

export const COMPOSITE_OPENED_AT = 'compositeOpenedAt'; 
export const COMPOSITE_CLOSED_AT = 'compositeClosedAt'

export const TradeActions = Object.freeze({
  ADD: "Add",
  REDUCE: "Reduce",
  EVALUATE: "Evaluate",
  CLOSE: "Close",
});

export const TradeStatus = Object.freeze({
  ANIDEA: "AnIdea",
  OPEN: "Open",
  CLOSED: "Closed",
});

//element
export const ELEMENT_COMPOSITEFK_STING = 'compositeFK'
export const ELEMENT_TYPE_STING = 'tradeActionType'
export const ELEMENT_TIMESTAMP_STING = 'timeStamp'

export const ElementActions = Object.freeze({
  TIMESTAMP: "TimeStamp",
  DELETE: "Delete",
});

export const ElementType = Object.freeze({
  ORIGIN: "Origin",
  ADD: "Add",
  REDUCE: "Reduce",
  EVALUATION: "Evaluation",
  SUMMARY: "Summary",
});

//entry
export const DATA_TITLE_STRING = 'title'
export const DATA_RELEVANT_FOR_ORVERVIEW_STRING = 'isRelevantForTradeOverview'
export const DATA_RELEVANT_FOR_LOCAL_ORVERVIEW_STRING = 'isRelevantForLocalOverview'
export const COMPONENT_TYPE ='componentType'
export const DATA_COMPOSITEFK_STRING = 'compositeFK'
export const DATA_TRADE_ELEMENTFK_STRING = 'tradeElementFK'

export const OverviewType = Object.freeze({
  NONE: "None",
  TRADE_OVERVIEW: "TradeOverview",
  ELEMENT_OVERVIEW: "ElementOverview",
});

//respones
export const NEW_DATA_RESPONSE_TAG = 'updatedCellData'
export const NEW_ELEMENT_RESPONSE_TAG = 'updatedElement'