import React from "react";
import * as Constants from "@constants/journalConstants";
import TradeElement from "@journalComponents/TradeElement";
import CompositeControls from "./CompositeControls";
import { useGetTradeById } from "@hooks/useGetTradeById";
import { useTradeStateAndManagement } from "@hooks/Composite/useTradeStateAndManagement";

function TradeExpanded({ tradeId }) {
  const { trade } = useGetTradeById(tradeId);
  const { tradeSummary, processEntryUpdate, processTradeAction } =
    useTradeStateAndManagement(trade);

  return (
    <>
      <ul>
        {trade[Constants.TRADE_ELEMENTS_STRING].map((ele, index) => (
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
        tradeComposite={trade}
        onTradeActionExecuted={processTradeAction}
      />
    </>
  );
}

export default React.memo(TradeExpanded);
