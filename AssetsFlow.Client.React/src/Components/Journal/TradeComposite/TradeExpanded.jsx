import React from "react";
import * as Constants from "@constants/journalConstants";
import TradeElementContainer from "@journalComponents/TradeElement/TradeElementContainer";
import CompositeControls from "./CompositeControls";
import { useGetTradeById } from "@hooks/useGetTradeById";
import { useTradeStateAndManagement } from "@hooks/Composite/useTradeStateAndManagement";

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
  const { tradeSummary, processEntryUpdate, processTradeAction } =
    useTradeStateAndManagement(trade);

  return (
    <div style={styles.container}>
      <ul style={styles.list}>
        {trade[Constants.TRADE_ELEMENTS_STRING].map((ele, index) => (
          <li key={ele.id} style={styles.listItem}>
            <TradeElementContainer
              tradeElement={ele}
              onElementContentUpdate={processEntryUpdate}
              style={index !== 0 ? { marginTop: "10px" } : {}}
            />
          </li>
        ))}
      </ul>
      {tradeSummary && <TradeElementContainer tradeElement={{ ...tradeSummary, isOverview: true }} />}
      {trade[Constants.TRADE_STATUS_STRING] != Constants.TradeStatus.CLOSED && <CompositeControls
        tradeComposite={trade}
        onTradeActionExecuted={processTradeAction}
      />}
    </div>
  );
}

export default React.memo(TradeExpanded);
