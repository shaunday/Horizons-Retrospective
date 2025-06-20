import React from "react";
import { useQuery } from "@tanstack/react-query";
import { tradeKeysFactory } from "@services/query-key-factory";
import TradeWrapper from "./TradeComposite/TradeWrapper";

const styles = {
  container: {
    flex: 1,
    overflowY: "auto",
    maxHeight: "100vh",
  },
  list: {
    flexDirection: "column",
  },
};

function TradesGallery() {
  // 🔥 Subscribe to trade IDs query
  const { data: cachedTradeIds = [] } = useQuery({
    queryKey: tradeKeysFactory.tradeIdsKey,
    queryFn: () => [], // No fetch needed, just cache tracking
  });

  return (
    <div style={styles.container} className="m-1 p-1">
      <ul style={styles.list} className="m-1">
        {cachedTradeIds.map((tradeId, index) => {
          const indexType = index % 2 === 0 ? 0 : 1;
          return (
            <li key={tradeId}>
              <TradeWrapper tradeId={tradeId} indexType={indexType} />
            </li>
          );
        })}
      </ul>
    </div>
  );
}

export default React.memo(TradesGallery);
