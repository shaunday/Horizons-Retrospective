import React from "react";
import { useQuery } from "@tanstack/react-query";
import { tradeKeysFactory } from "@services/query-key-factory";
import TradeWrapper from "./TradeComposite/TradeWrapper";

// eslint-disable-next-line react/prop-types
function TradesGallery({ newlyAddedTradeId }) {
  const { data: cachedTradeIds = [] } = useQuery({
    queryKey: tradeKeysFactory.getKeyForAllTrades(),
    queryFn: () => [],
  });
  return (
    <div className="m-1 p-1 max-h-screen flex-1 overflow-y-auto">
      <ul className="m-1 flex flex-col">
        {cachedTradeIds.map((trade) => {
          return (
            <li key={trade.id}>
              <TradeWrapper tradeId={trade.id} isNewTrade={newlyAddedTradeId === trade.id} />
            </li>
          );
        })}
      </ul>
    </div>
  );
}

export default React.memo(TradesGallery);
