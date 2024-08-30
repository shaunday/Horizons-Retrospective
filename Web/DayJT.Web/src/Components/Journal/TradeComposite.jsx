import { useState } from "react";
import TradeElement from "./TradeElement";
import CompositeControls from "./CompositeControls";
import { useCacheUpdatedEntry } from "@hooks/useTradeUpdate";
import { useCacheNewElement } from "@hooks/useCacheNewElement";
import { useQuery } from "@tanstack/react-query";
import { getTradeQueryConfig } from "@servies/tradeQueryHelper"; //
import * as Constants from "@constants/journalConstants";

function TradeComposite({ tradeComposite }) {
  const tradeQueryConfig = getTradeQueryConfig(
    tradeComposite[Constants.TRADE_CLIENT_ID_PROPERTY],
    tradeComposite
  );
  const { cachedTradeComposite } = useQuery(tradeQueryConfig);

  const [tradeSummary, setTradeSummary] = useState(
    useCacheTradeComposite[Constants.TRADE_SUMMARY_STRING]
  );

  const entryUpdate = useCacheUpdatedEntry(useCacheTradeComposite);
  const processEntryUpdate = useCallback(({ updatedEntry, newSummary }) => {
    entryUpdate(updatedEntry);
    setTradeSummary(newSummary);
  }, []);

  const { onElementUpdate } = useCacheNewElement(cachedTradeComposite);
  const processTradeAction = useCallback(({ newElement, newSummary }) => {
    if (!!newElement) {
      onElementUpdate(newElement);
    }
    setTradeSummary(newSummary);
  }, []);

  return (
    <>
      <ul>
        {cachedTradeComposite[Constants.TRADE_ELEMENTS_STRING].map((ele) => (
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
        tradeComposite={cachedTradeComposite}
        onTradeActionExecuted={processTradeAction}
      />
    </>
  );
}

export default React.memo(TradeComposite);
