import { useState, useCallback } from "react";
import * as Constants from "@constants/journalConstants";
import { useCacheUpdatedEntry } from "@hooks/Journal/Entry/useCacheUpdatedEntry";
import { useCacheNewElement } from "@hooks/Journal/Element/useCacheNewElement";
import { newStatesResponseParser } from "@services/newStatesResponseParser"

export function useTradeStateAndManagement(cachedTradeComposite) {
  const [tradeSummary, setTradeSummary] = useState(
    cachedTradeComposite?.[Constants.TRADE_SUMMARY_STRING] ?? null
  );

  const { cacheUpdatedEntry } = useCacheUpdatedEntry(cachedTradeComposite);
  const processEntryUpdate = useCallback(
    (cellUpdateResponse) => {
      const updatedEntry = cellUpdateResponse[Constants.NEW_DATA_RESPONSE_TAG];
      const { newSummary } = newStatesResponseParser(cellUpdateResponse);

      cacheUpdatedEntry(updatedEntry);
      if (newSummary) {
        processSummaryUpdate(newSummary);
      }
    },
    [cacheUpdatedEntry]
  );

  const processSummaryUpdate = useCallback(
    (newSummary) => {
      setTradeSummary(newSummary);
    }
  );

  const { onElementUpdate } = useCacheNewElement(cachedTradeComposite);
  const processTradeAction = useCallback(
    (newElement, newSummary) => {
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
    processSummaryUpdate,
  };
}
