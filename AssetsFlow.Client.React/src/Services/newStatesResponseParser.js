const NEW_STATES_RESPONSE_TAG = 'updatedStates'
const ELEMENT_TIMESTAMP = 'formattedElementTimeStamp';
const COMPOSITE_INFO_TAG = 'tradeInfo';
const SUMMARY = 'summary';
const TRADE_STATUS = 'tradeStatus'; 
const TRADE_ISPENDING = 'isPending'; 
const COMPOSITE_OPENED_AT = 'compositeOpenedAt'; 
const COMPOSITE_CLOSED_AT = 'compositeClosedAt'
const SAVED_SECTORS = 'savedSectors'

export const newStatesResponseParser = (response) => {
    const updatedStates = response?.[NEW_STATES_RESPONSE_TAG];
    const tradeInfo = updatedStates?.[COMPOSITE_INFO_TAG];

    return {
        elementsNewTimeStamp: updatedStates?.[ELEMENT_TIMESTAMP],
        newSummary: tradeInfo?.[SUMMARY],

        newTradeStatus: tradeInfo?.[TRADE_STATUS],
        tradeIsPending: tradeInfo?.[TRADE_ISPENDING],

        tradeOpenedAt: tradeInfo?.[COMPOSITE_OPENED_AT],
        tradeClosedAt: tradeInfo?.[COMPOSITE_CLOSED_AT],
        
        savedSectors: updatedStates?.[SAVED_SECTORS],
    };
};
