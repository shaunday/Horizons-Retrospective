//trade
export const TRADE_SUMMARY_STRING = 'summary';
export const TRADE_ELEMENTS_STRING = 'tradeElements'
export const TRADE_ENTRIES_STRING = 'entries'
export const TRADE_STATUS_STRING = 'status'

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
  ACTIVATE: "Activate",
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
export const DATA_RELEVANT_FOR_ORVERVIEW_STRING = 'isRelevantForOverview'
export const DATA_RESTRICTION_STRING = 'restrictions'
export const COMPONENT_TYPE ='componentType'

//respones
export const NEW_DATA_RESPONSE_TAG = 'updatedCellData'
export const NEW_SUMMARY_RESPONSE_TAG = 'updatedSummary'
export const NEW_ELEMENT_RESPONSE_TAG = 'updatedElement'
export const NEW_TIMESTAMP_RESPONSE_TAG = 'updatedTimeStamp'

