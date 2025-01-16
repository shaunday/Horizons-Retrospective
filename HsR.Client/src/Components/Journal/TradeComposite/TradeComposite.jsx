import React from "react";
import * as Constants from "@constants/journalConstants";
import TradeElement from "@journalComponents/TradeElement";
import CompositeControls from "./CompositeControls";
import { useTrade } from "@hooks/useTrade";
import { useTradeStateAndManagement } from "@hooks/Composite/useTradeStateAndManagement";

function TradeComposite({ tradeId }) {
  const { cachedTradeComposite } = useTrade(tradeId);
  const { tradeSummary, processEntryUpdate, processTradeAction } =
    useTradeStateAndManagement(cachedTradeComposite);

  return (
    <>
      <ul>
        {cachedTradeComposite[Constants.TRADE_ELEMENTS_STRING].map((ele) => (
          <li key={ele.id}>
            <TradeElement
              tradeElement={ele}
              onElementContentUpdate={processEntryUpdate}
              style={index !== 0 ? { marginTop: "10px" } : {}}
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
