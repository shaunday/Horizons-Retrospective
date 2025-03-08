import React from "react";
import * as Constants from "@constants/journalConstants";
import TradeElement from "@journalComponents/TradeElement/TradeElement";
import CompositeControls from "./Controls/CompositeControls";
import { useGetTradeById } from "@hooks/Journal/useGetTradeById";
import { useTradeStateAndManagement } from "@hooks/Journal/Composite/useTradeStateAndManagement";

const styles = {
  container: {
    display: "flex",
    flexDirection: "column", // Stack children vertically
    listStyleType: "none", // Removes default list bullets
    alignItems: "flex-start", // Align children to the left
  },
  listItem: {
    padding: "5px",
    borderRadius: "4px",
    margin: "3px",
    border: "1.5px solid purple"
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
  }

  return (
    <div style={styles.container}>
      <ul style={{ flexDirection: "column" }}>
        {trade[Constants.TRADE_ELEMENTS_STRING].map((ele) => (
          <li key={ele.id} style={styles.listItem}>
            <TradeElement
              tradeElement={ele}
              onElementContentUpdate={processEntryUpdate}
              onElementAction={processElementAction}
            />
          </li>
        ))}
      </ul>
      {tradeSummary && <TradeElement tradeElement={{ ...tradeSummary, isOverview: true }} />}
      {trade[Constants.TRADE_STATUS_STRING] != Constants.TradeStatus.CLOSED &&
       <CompositeControls
        tradeComposite={trade}
        onTradeActionExecuted={processTradeAction}
      />}
    </div>
  );
}

export default React.memo(TradeExpanded);
