import { useState } from "react";
import * as Constants from "@constants/constants";
import { getTradeById, addTradeComposite } from "@services/tradesApiAccess";
import { useMutation, useQueries, useQueryClient } from "@tanstack/react-query";
import { getTradeQueryConfig } from "@services/tradeQueryHelper";

function useTrades(tradeClientIds) {
  const queryClient = useQueryClient();
  const [tradeIdCounter, setTradeIdCounter] = useState(0);

  // Define queries for fetching trades using getTradeQueryConfig
  const queries = tradeClientIds.map((clientId) =>
    getTradeQueryConfig(clientId)
  );

  // Execute all queries
  const results = useQueries({ queries });

  // Extract trade data from the results
  const trades = results
    .map((result) => result.data)
    .filter((data) => data !== undefined);

  // Mutation for adding a trade
  const { mutate: addTrade, isLoading: isAddingTrade } = useMutation(
    async () => {
      const trade = await addTradeComposite();
      return trade;
    },
    {
      onSuccess: (data) => {
        if (data) {
          queryClient.setQueryData([Constants.TRADE_KEY, tradeIdCounter], data);
          setTradeIdCounter((prevId) => prevId + 1); // Increment the counter
        }
      },
    }
  );

  return {
    trades,
    addTrade,
    isAddingTrade,
  };
}

export default useTrades;
