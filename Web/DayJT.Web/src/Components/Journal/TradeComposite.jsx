import { useState } from "react";
import TradeElement from "./TradeElement";
import CompositeControls from "./CompositeControls";
import { useCacheUpdatedEntry } from "@hooks/useTradeUpdate";
import { useCacheNewElement } from "@hooks/useCacheNewElement";

import * as Constants from "@constants/journalConstants";

function TradeComposite({ tradeComposite }) {
  const initialElementsValue = tradeComposite[Constants.TRADE_ELEMENTS_STRING];

  const [tradeSummary, setTradeSummary] = useState(
    tradeComposite[Constants.TRADE_SUMMARY_STRING]
  );

  const entryUpdate = useCacheUpdatedEntry(tradeComposite);
  const processEntryUpdate = useCallback(({ updatedEntry, newSummary }) => {
    entryUpdate(updatedEntry);
    setTradeSummary(newSummary);
  }, []);

  const { onElementUpdate } = useCacheNewElement();
  const processTradeAction = useCallback(({ newElement, newSummary }) => {
    if (!!newElement) {
      onElementUpdate(newElement);
    }
    setTradeSummary(newSummary);
  }, []);

  return (
    <>
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
      {tradeSummary && <TradeElement tradeElement={tradeSummary} />}
      <CompositeControls
        tradeComposite={tradeComposite}
        onTradeActionExecuted={processTradeAction}
      />
    </>
  );
}

export default React.memo(TradeComposite);
