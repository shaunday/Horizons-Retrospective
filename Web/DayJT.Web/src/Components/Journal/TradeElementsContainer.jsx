import React, { useCallback, useState } from "react";
import TradeElement from "./TradeElement";
import { useCacheUpdatedEntry } from "@hooks/useTradeUpdate";
import { useCacheNewElement } from "@hooks/useCacheNewElement";
import * as Constants from "@constants/journalConstants";

function TradeElementsList({
  tradeComposite,
  initialElementsValue,
  onTradeSummaryUpdate,
}) {
  const [tradeSummary, setTradeSummary] = useState(
    tradeComposite[Constants.TRADE_SUMMARY_STRING]
  );

  const entryUpdate = useCacheUpdatedEntry(tradeComposite);

  const processEntryUpdate = useCallback(
    ({ updatedEntry, newSummary }) => {
      entryUpdate(updatedEntry);
      setTradeSummary(newSummary);
      onTradeSummaryUpdate(newSummary);
    },
    [entryUpdate, onTradeSummaryUpdate]
  );

  const { onElementUpdate } = useCacheNewElement(tradeComposite);

  const processTradeAction = useCallback(
    ({ newElement, newSummary }) => {
      if (!!newElement) {
        onElementUpdate(newElement);
      }
      setTradeSummary(newSummary);
      onTradeSummaryUpdate(newSummary);
    },
    [onElementUpdate, onTradeSummaryUpdate]
  );

  return (
    <ul>
      {initialElementsValue.map((ele) => (
        <li key={ele.id}>
          <TradeElement
            tradeElement={ele}
            onElementContentUpdate={processEntryUpdate}
          />
        </li>
      ))}
    </ul>
  );
}

export default React.memo(TradeElementsList);
