import React from "react";
import * as Constants from "@constants/journalConstants";
import TradeElement from "@journalComponents/TradeElement/TradeElement";
import CompositeControls from "./Controls/CompositeControls";
import { useTradeStateAndManagement } from "@hooks/Journal/Composite/useTradeStateAndManagement";
import TradeElementBadge from "../TradeElement//TradeElementBadge"; 
import { newStatesResponseParser } from "@services/newStatesResponseParser"

const styles = {
  listItem: {
    padding: "10px",
    borderRadius: "6px",
    margin: "8px 4px",
    border: "1.5px solid purple",
    position: "relative",
  },
};

function TradeExpanded({ trade }) {
  const { tradeSummary, processEntryUpdate, processTradeAction, processSummaryUpdate } =
    useTradeStateAndManagement(trade);

  const processElementAction = (action, response) => {
    if (action === Constants.ElementActions.DELETE) {
      const { newSummary } = newStatesResponseParser(response);
      if (newSummary) {
        processSummaryUpdate(newSummary);
      }
    }
  };

  return (
    <div>
      <ul style={{ flexDirection: "column" }}>
        {trade[Constants.TRADE_ELEMENTS_STRING].map((ele) => (
          <li key={ele.id} style={styles.listItem}>
            <TradeElementBadge tradeElement={ele} />
            <TradeElement
              tradeElement={ele}
              onElementContentUpdate={processEntryUpdate}
              onElementAction={processElementAction}
            />
          </li>
        ))}
      </ul>
      {tradeSummary && <TradeElement tradeElement={{ ...tradeSummary, isOverview: true }} />}
      {trade[Constants.TRADE_STATUS_STRING] !== Constants.TradeStatus.CLOSED && (
        <CompositeControls tradeComposite={trade} onTradeActionExecuted={processTradeAction} />
      )}
    </div>
  );
}

export default React.memo(TradeExpanded);