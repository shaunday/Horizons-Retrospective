import React from "react";
import { tradeKeysFactory } from "@services/query-key-factory";
import TradeWrapper from "./TradeComposite/TradeWrapper";
import { useQueryClient } from "@tanstack/react-query";

const styles = {
  list: {
    display: "flex",
    listStyleType: "none", // Removes default list bullets
    flexDirection: "column",
    padding: 0, // Removes default padding
    margin: "2px", // Removes default margin
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
    <ul style={styles.list}>
      {cachedTradeIds.map((tradeId, index) => (
        <li key={tradeId} style={styles.listItem}>
          <TradeWrapper tradeId={tradeId}  
          style={index !== 0 ? { marginTop: "10px" } : {}}/>
        </li>
      ))}
    </ul>
  );
}

export default React.memo(TradesContainer);
