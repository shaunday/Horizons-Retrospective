import React from "react";
import { useQuery } from "@tanstack/react-query";
import { tradeKeysFactory } from "@services/query-key-factory";
import TradeWrapper from "./TradeComposite/TradeWrapper";

const styles = {
  container: {
    flex: 1,
    overflowY: "auto",
    maxHeight: "100vh",
    borderRadius: "4px",
    margin: "3px",
    padding: "5px",
    background: "rgb(241, 243, 237)",
  },
  list: {
    flexDirection: "column",
    margin: "3px",
  },
};

function TradesGallery() {
  // 🔥 Subscribe to trade IDs query
  const { data: cachedTradeIds = [] } = useQuery({
    queryKey: tradeKeysFactory.tradeIdsKey,
    queryFn: () => [], // No fetch needed, just cache tracking
  });

  return (
    <div style={styles.container}>
      <ul style={styles.list}>
        {cachedTradeIds.map((tradeId) => (
          <li key={tradeId}>
            <TradeWrapper tradeId={tradeId} />
          </li>
        ))}
      </ul>
    </div>
  );
}

export default React.memo(TradesGallery);
