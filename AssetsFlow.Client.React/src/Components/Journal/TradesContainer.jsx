import React from "react";
import { tradeKeysFactory } from "@services/query-key-factory";
import TradeWrapper from "./TradeComposite/TradeWrapper";
import { useQueryClient } from "@tanstack/react-query";

const styles = {
  container: {
    flex: 1,
    overflowY: "auto",
    maxHeight: "100vh", 
    margin: "3px",
    padding: "5px",
    background: "rgb(225, 167, 167)",
  },
  list: {
    display: "flex",
    listStyleType: "none",
    flexDirection: "column",
    padding: 0,
    margin: "2px",
  },
  listItem: {
    border: "1.5px solid blue",
    padding: "5px",
    borderRadius: "4px",
    marginBottom: "5px",
  },
};

function TradesContainer() {
  const queryClient = useQueryClient();

  // Get the list of trade IDs from the query cache
  const cachedTradeIds =
    queryClient.getQueryData(tradeKeysFactory.tradeIdsKey) || [];

  return (
    <div style={styles.container}>
    <ul style={styles.list}>
      {cachedTradeIds.map((tradeId, index) => (
        <li key={tradeId} style={styles.listItem}>
          <TradeWrapper tradeId={tradeId}  
          style={index !== 0 ? { marginTop: "10px" } : {}}/>
        </li>
      ))}
    </ul>
  </div>
  );
}

export default React.memo(TradesContainer);
