import React from "react";
import * as Constants from "@constants/journalConstants";
import TradeElement from "@journalComponents/TradeElement/TradeElement";
import CompositeControls from "./Controls/CompositeControls";
import { useGetTradeById } from "@hooks/Journal/useGetTradeById";
import { useTradeStateAndManagement } from "@hooks/Journal/Composite/useTradeStateAndManagement";
import TradeElementBadge from "../TradeElement//TradeElementBadge"; 

const styles = {
  listItem: {
    padding: "10px",
    borderRadius: "6px",
    margin: "8px 0",
    border: "1.5px solid purple",
    position: "relative",
  },
  compositeControlsContainer: {
    display: "flex",
    justifyContent: "center",
    width: "100%",
  },
};

function TradeExpanded({ tradeId }) {
  const { trade } = useGetTradeById(tradeId);
  const { tradeSummary, processEntryUpdate, processTradeAction, processSummaryUpdate } =
    useTradeStateAndManagement(trade);

  const processElementAction = (action, response) => {
    if (action === Constants.ElementActions.DELETE) {
      const updatedSummary = response[Constants.NEW_SUMMARY_RESPONSE_TAG];
      if (updatedSummary) {
        processSummaryUpdate(updatedSummary);
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
        <div style={styles.compositeControlsContainer}>
          <CompositeControls tradeComposite={trade} onTradeActionExecuted={processTradeAction} />
        </div>
      )}
    </div>
  );
}

export default React.memo(TradeExpanded);