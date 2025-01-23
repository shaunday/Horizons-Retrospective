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

  const containerStyle = {
    display: "flex",
    flexDirection: "column", // Stack children vertically
    alignItems: "flex-start", // Optional: Align children to the left (default)
  };

  return (
    <div style={containerStyle}>
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
      {tradeSummary && <TradeElement tradeElement={tradeSummary}/>}
      <CompositeControls
        tradeComposite={trade}
        onTradeActionExecuted={processTradeAction}
      />
    </div>
  );
}

export default React.memo(TradeExpanded);
