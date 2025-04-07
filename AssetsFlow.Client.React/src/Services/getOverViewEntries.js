import * as Constants from "@constants/journalConstants";

export function getOverViewEntries(elementsToFlatten, relevanceKey) {
    const simulatedEntries = elementsToFlatten.flatMap((tradeElement) =>
      tradeElement[Constants.TRADE_ENTRIES_STRING].filter(
        (entry) => entry[relevanceKey]
      )
    );
  
    return simulatedEntries;
  }  