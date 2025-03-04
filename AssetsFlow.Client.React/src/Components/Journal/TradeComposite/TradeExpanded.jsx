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
  list: {
    listStyleType: "none", // Removes default list bullets
    padding: 0, // Removes default padding
    margin: 0, // Removes default margin
  },
  listItem: {
    border: "1.5px solid purple",
    padding: "5px",
    borderRadius: "4px",
    margin: "5px",
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
      <ul style={styles.list}>
        {trade[Constants.TRADE_ELEMENTS_STRING].map((ele, index) => (
          <li key={ele.id} style={styles.listItem}>
            <TradeElement
              tradeElement={ele}
              onElementContentUpdate={processEntryUpdate}
              onElementAction={processElementAction}
              style={index !== 0 ? { marginTop: "10px" } : {}}
            />
          </li>
        ))}
      </ul>
      {tradeSummary && <TradeElement tradeElement={{ ...tradeSummary, isOverview: true }} />}
      {trade[Constants.TRADE_STATUS_STRING] != Constants.TradeStatus.CLOSED && <CompositeControls
        tradeComposite={trade}
        onTradeActionExecuted={processTradeAction}
      />}
    </div>
  );
}

export default React.memo(TradeExpanded);
