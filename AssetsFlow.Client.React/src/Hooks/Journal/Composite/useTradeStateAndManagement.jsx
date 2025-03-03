import { useState, useCallback } from "react";
import * as Constants from "@constants/journalConstants";
import { useCacheUpdatedEntry } from "@hooks/Journal/Entry/useCacheUpdatedEntry";
import { useCacheNewElement } from "@hooks/Journal/Element/useCacheNewElement";

export function useTradeStateAndManagement(cachedTradeComposite) {
  const [tradeSummary, setTradeSummary] = useState(
    cachedTradeComposite[Constants.TRADE_SUMMARY_STRING] ?? null
  );

  const { cacheUpdatedEntry } = useCacheUpdatedEntry(cachedTradeComposite);
  const processEntryUpdate = useCallback(
    (cellUpdateResponse) => {
      const updatedEntry = cellUpdateResponse[Constants.NEW_DATA_RESPONSE_TAG];
      const newSummary = cellUpdateResponse[Constants.NEW_SUMMARY_RESPONSE_TAG];

      cacheUpdatedEntry(updatedEntry);
      setTradeSummary(newSummary);
    },
    [cacheUpdatedEntry]
  );

  const { onElementUpdate } = useCacheNewElement(cachedTradeComposite);
  const processTradeAction = useCallback(
    ( newElement, newSummary ) => {
      if (newElement) {
        onElementUpdate(newElement);
      }
      if (newSummary) {
        setTradeSummary(newSummary);
      }
    },
    [onElementUpdate]
  );

  return {
    tradeSummary,
    processEntryUpdate,
    processTradeAction,
  };
}
