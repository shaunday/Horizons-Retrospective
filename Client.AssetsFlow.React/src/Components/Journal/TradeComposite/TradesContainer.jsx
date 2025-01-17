import React from "react";
import * as Constants from "@constants/journalConstants";
import TradeComposite from "./TradeComposite";
import { useQueryClient } from "@tanstack/react-query";

function TradesContainer() {
  const queryClient = useQueryClient();

  // Get the list of trade IDs from the query cache
  const cachedTradeIds =
    queryClient.getQueryData([Constants.TRADE_IDS_ARRAY_KEY]) || [];

  return (
    <ul>
      {cachedTradeIds.map((tradeId) => (
        <li key={tradeId}>
          <TradeComposite tradeId={tradeId} />
        </li>
      ))}
    </ul>
  );
}

export default React.memo(TradesContainer);
