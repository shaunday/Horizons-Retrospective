import { useState, useCallback } from "react";
import * as Constants from "@constants/journalConstants";
import { useCacheUpdatedEntry } from "@hooks/Journal/Entry/useCacheUpdatedEntry";
import { useCacheNewElement } from "@hooks/Journal/Element/useCacheNewElement";
import { newStatesResponseParser } from "@services/newStatesResponseParser"
import { useHandleStatusUpdates } from "./useHandleStatusUpdates";

export function useTradeStateAndManagement(cachedTradeComposite) {
  const [tradeSummary, setTradeSummary] = useState(
    cachedTradeComposite?.[Constants.TRADE_SUMMARY_STRING] ?? null
  );
  const { onElementUpdate } = useCacheNewElement(cachedTradeComposite);
  const { cacheUpdatedEntry } = useCacheUpdatedEntry(cachedTradeComposite);
  const processTradeStatusUpdates = useHandleStatusUpdates(cachedTradeComposite);

  const processEntryUpdate = useCallback(
    (cellUpdateResponse) => {
      const updatedEntry = cellUpdateResponse[Constants.NEW_DATA_RESPONSE_TAG];
      const { newSummary } = newStatesResponseParser(cellUpdateResponse);

      cacheUpdatedEntry(updatedEntry);
      if (newSummary) {
        processSummaryUpdate(newSummary);
      }
      processTradeStatusUpdates(cellUpdateResponse);
    },
    [cacheUpdatedEntry]
  );

  const processSummaryUpdate = useCallback(
    (newSummary) => {
      setTradeSummary(newSummary);
    }
  );

  const processTradeAction = useCallback(
    (response) => {
      processTradeStatusUpdates(response);

      const newElement = response[Constants.NEW_ELEMENT_RESPONSE_TAG];
      const { newSummary } = newStatesResponseParser(response);

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
