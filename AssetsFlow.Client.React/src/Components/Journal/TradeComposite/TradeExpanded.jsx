import React from "react";
import * as Constants from "@constants/journalConstants";
import CompositeControls from "./Controls/CompositeControls";
import { useTradeStateAndManagement } from "@hooks/Journal/Composite/useTradeStateAndManagement";
import { newStatesResponseParser } from "@services/newStatesResponseParser"
import TradeElementWrapper from "../TradeElement/TradeElementWrapper";
import TradeElementCollapsed from "../TradeElement/TradeElementCollapsed";

function TradeExpanded({ trade }) {
  const { tradeSummary, processEntryUpdate, processTradeAction, processSummaryUpdate } =
    useTradeStateAndManagement(trade);

  const processElementAction = (action, response) => {
    const { newSummary } = newStatesResponseParser(response);
    processSummaryUpdate(newSummary);
  };

  const gapValue = 7;

  return (
    <div>
      <ul style={{ display: "flex", flexDirection: "column" }}>
        {trade[Constants.TRADE_ELEMENTS_STRING].map((ele, index, arr) => (
          <li
            key={ele.id}
            style={{
              marginBottom: index !== arr.length - 1 ? gapValue : 0,
            }}
          >
            <TradeElementWrapper
              tradeElement={ele}
              onElementContentUpdate={processEntryUpdate}
              onElementAction={processElementAction}
            />
          </li>
        ))}
      </ul>

      {tradeSummary && (
        <div style={{marginLeft: 15, marginTop: gapValue }}>
          <TradeElementCollapsed tradeElement={tradeSummary} />
        </div>
      )}

      {trade[Constants.TRADE_STATUS] !== Constants.TradeStatus.CLOSED && (
        <CompositeControls
          tradeComposite={trade}
          onTradeActionSuccess={processTradeAction}
        />
      )}
    </div>
  );
}

export default React.memo(TradeExpanded);