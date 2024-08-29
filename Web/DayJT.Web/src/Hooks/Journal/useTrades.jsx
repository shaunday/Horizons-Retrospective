import { useState } from "react";
import * as Constants from "@constants/constants";
import { getTradeById, addTradeComposite } from "@services/tradesApiAccess";
import { useQueries, useQuery, useQueryClient } from "@tanstack/react-query";

function useTrades(tradeClientIds, isAddTrade, onTradeAdded) {
  const queryClient = useQueryClient();
  const [tradeIdCounter, setTradeIdCounter] = useState(0);
  const [isAddingTrade, setIsAddingTrade] = useState(false);

  // Function to update the trade data in the cache
  function saveTrade(clientId, data) {
    const updatedData = {
      ...data,
      [Constants.TRADE_CLIENTID_PROP_STRING]: clientId,
    };

    // Update the cache with the new data
    queryClient.setQueryData([Constants.TRADE_KEY, clientId], updatedData);
  }

  // Define queries for fetching trades
  const queries = tradeClientIds.map((clientId) => ({
    queryKey: [Constants.TRADE_KEY, clientId],
    queryFn: async () => await getTradeById(clientId),
    onSuccess: (data) => saveTrade(clientId, data),
    keepPreviousData: true,
  }));

  // Execute all queries
  const results = useQueries(queries);

  // Extract trade data from the results
  const trades = results
    .map((result) => result.data)
    .filter((data) => data !== undefined);

  // useQuery for adding a trade, based on `isAddTrade` flag
  const { data: addTradeData } = useQuery({
    queryKey: [Constants.TRADE_KEY, tradeIdCounter],
    queryFn: async () => {
      setIsAddingTrade(true); // Start adding trade
      const trade = await addTradeComposite();
      setIsAddingTrade(false); // Finish adding trade
      return trade;
    },
    enabled: isAddTrade && !isAddingTrade,
    onSuccess: (data) => {
      if (data) {
        saveTrade(tradeIdCounter, data);
        // Call onTradeAdded callback
        onTradeAdded();
        setTradeIdCounter((prevId) => prevId + 1); // Increment the counter
      }
    },
    onError: () => {
      setIsAddingTrade(false); // Ensure state is reset on error
    },
  });

  return {
    trades,
    addTradeData,
    isAddingTrade,
  };
}

export default useTrades;
