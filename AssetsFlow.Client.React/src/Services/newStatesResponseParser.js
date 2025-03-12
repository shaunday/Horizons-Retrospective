const NEW_STATES_RESPONSE_TAG = 'updatedStates'
const ELEMENT_TIMESTAMP = 'formattedElementTimeStamp';
const SUMMARY = 'summary';
const TRADE_STATUS = 'TradeStatus'; 
const COMPOSITE_OPENED_AT = 'compositeOpenedAt'; 
const COMPOSITE_CLOSED_AT = 'compositeClosedAt'

export const newStatesResponseParser = (response) => {
    return {
        elementsNewTimeStamp: response?.[NEW_STATES_RESPONSE_TAG]?.[ELEMENT_TIMESTAMP] ?? {},
        newSummary: response?.[NEW_STATES_RESPONSE_TAG]?.[ELEMENT_TIMESTAMP] ?? {},
        newTradeStatus: response?.[NEW_STATES_RESPONSE_TAG]?.[SUMMARY] ?? {},
        tradeOpenedAt: response?.[NEW_STATES_RESPONSE_TAG]?.[TRADE_STATUS] ?? {},
        tradeClosedAt: response?.[NEW_STATES_RESPONSE_TAG]?.[COMPOSITE_OPENED_AT] ?? {},
        tradeClosedAt: response?.[NEW_STATES_RESPONSE_TAG]?.[COMPOSITE_CLOSED_AT] ?? {},
    };
};