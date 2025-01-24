import { useState, useCallback } from "react";
import { useCacheUpdatedEntry } from "@hooks/Entry/useCacheUpdatedEntry";
import { useCacheNewElement } from "@hooks/Element/useCacheNewElement";
import * as Constants from "@constants/journalConstants";

export function useTradeStateAndManagement(cachedTradeComposite) {
  const [tradeSummary, setTradeSummary] = useState(
    cachedTradeComposite[Constants.TRADE_SUMMARY_STRING] ?? null
  );

  const entryUpdate = useCacheUpdatedEntry(cachedTradeComposite);
  const processEntryUpdate = useCallback(
    ({ updatedEntry, newSummary }) => {
      entryUpdate(updatedEntry);
      setTradeSummary(newSummary);
    },
    [entryUpdate]
  );

  const { onElementUpdate } = useCacheNewElement(cachedTradeComposite);
  const processTradeAction = useCallback(
    ({ newElement, newSummary }) => {
      if (newElement) {
        onElementUpdate(newElement);
      }
      setTradeSummary(newSummary);
    },
    [onElementUpdate]
  );

  return {
    tradeSummary,
    processEntryUpdate,
    processTradeAction,
  };
}
