import React from "react";
import TradeWrapper from "./TradeComposite/TradeWrapper";
import { useFetchAndCacheTrades } from "@hooks/Journal/useFetchAndCacheTrades";

// eslint-disable-next-line react/prop-types
function TradesGallery({ newlyAddedTradeId }) {
  const { trades: cachedTradeIds } = useFetchAndCacheTrades();
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
