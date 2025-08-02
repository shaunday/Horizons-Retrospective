import React from "react";
import { useQuery } from "@tanstack/react-query";
import { tradeKeysFactory } from "@services/query-key-factory";
import TradeWrapper from "./TradeComposite/TradeWrapper";

function TradesGallery({ newlyAddedTradeId }) {
  const { data: cachedTradeIds = [] } = useQuery({
    queryKey: tradeKeysFactory.getKeyForAllTradesByIds(),
    queryFn: () => [],
  });
  return (
    <div className="m-1 p-1 max-h-screen flex-1 overflow-y-auto">
      <ul className="m-1 flex flex-col">
        {cachedTradeIds.map((tradeId) => {
          return (
            <li key={tradeId}>
              <TradeWrapper 
                tradeId={tradeId} 
                isNewTrade={newlyAddedTradeId === tradeId}
              />
            </li>
          );
        })}
      </ul>
    </div>
  );
}

export default React.memo(TradesGallery);
