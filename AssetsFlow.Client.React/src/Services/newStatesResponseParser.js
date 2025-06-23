const NEW_STATES_RESPONSE_TAG = 'updatedStates'
const ELEMENT_TIMESTAMP = 'activationTimeStamp';
const COMPOSITE_INFO_TAG = 'tradeInfo';
const SAVED_SECTORS = 'savedSectors'
import * as Constants from "@constants/journalConstants";

export const newStatesResponseParser = (response) => {
    const updatedStates = response?.[NEW_STATES_RESPONSE_TAG];
    const tradeInfo = updatedStates?.[COMPOSITE_INFO_TAG];

    return {
        elementsNewTimeStamp: updatedStates?.[ELEMENT_TIMESTAMP],
        
        newSummary: tradeInfo?.[Constants.TRADE_SUMMARY_STRING],
        newTradeStatus: tradeInfo?.[Constants.TRADE_STATUS],
        hasMissingContent: tradeInfo?.[Constants.HasMissingContent],

        tradeOpenedAt: tradeInfo?.[Constants.COMPOSITE_OPENED_AT],
        tradeClosedAt: tradeInfo?.[Constants.COMPOSITE_CLOSED_AT],
        
        savedSectors: updatedStates?.[SAVED_SECTORS],
    };
};
