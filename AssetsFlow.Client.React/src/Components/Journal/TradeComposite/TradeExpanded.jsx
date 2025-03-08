import React from "react";
import { Badge } from "@mantine/core";
import * as Constants from "@constants/journalConstants";
import TradeElement from "@journalComponents/TradeElement/TradeElement";
import CompositeControls from "./Controls/CompositeControls";
import { useGetTradeById } from "@hooks/Journal/useGetTradeById";
import { useTradeStateAndManagement } from "@hooks/Journal/Composite/useTradeStateAndManagement";

const styles = {
  listItem: {
    padding: "10px",
    borderRadius: "6px",
    margin: "8px 0",
    border: "1.5px solid purple",
    position: "relative",
  },
  badge: {
    position: "absolute",
    top: "-10px",
    left: "20px",
    zIndex: 10,
  },
  compositeControlsContainer: {
    display: "flex",
    justifyContent: "center",  // Center the child component horizontally
    width: "100%",  // Ensure the container takes full width
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
            {ele[Constants.ELEMENT_TIMESTAMP_STING] && (
              <Badge size="sm" color="blue.4" style={styles.badge}>
                {ele[Constants.ELEMENT_TIMESTAMP_STING]}
              </Badge>
            )}
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
          <CompositeControls
            tradeComposite={trade}
            onTradeActionExecuted={processTradeAction}
          />
        </div>
      )}
    </div>
  );
}

export default React.memo(TradeExpanded);
