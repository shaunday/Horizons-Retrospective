const NEW_STATES_RESPONSE_TAG = 'updatedStates'
const ELEMENT_TIMESTAMP = 'formattedElementTimeStamp';
const COMPOSITE_INFO_TAG = 'tradeInfo';
const SAVED_SECTORS = 'savedSectors'
import * as Constants from "@constants/journalConstants";

export const newStatesResponseParser = (response) => {
    const updatedStates = response?.[NEW_STATES_RESPONSE_TAG];
    const tradeInfo = updatedStates?.[COMPOSITE_INFO_TAG];

    return {
        elementsNewTimeStamp: updatedStates?.[ELEMENT_TIMESTAMP],
        
        newSummary: tradeInfo?.[Constants.SUMMARY],
        newTradeStatus: tradeInfo?.[Constants.TRADE_STATUS],
        tradeIsPending: tradeInfo?.[Constants.TRADE_ISPENDING],

        tradeOpenedAt: tradeInfo?.[Constants.COMPOSITE_OPENED_AT],
        tradeClosedAt: tradeInfo?.[Constants.COMPOSITE_CLOSED_AT],
        
        savedSectors: updatedStates?.[SAVED_SECTORS],
    };
};
