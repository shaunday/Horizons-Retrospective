import { useQuery, useQueryClient } from "@tanstack/react-query";
import * as Constants from "@constants/journalConstants";

// Replace with your actual function to fetch a trade by its ID
import { getTradeById } from "@services/tradesApiAccess";

function useTrade(tradeId) {
  const queryClient = useQueryClient();

  // Define the query key based on the trade ID
  const queryKey = [Constants.TRADE_KEY, tradeId];

  // Fetch the trade from the API and use the cache for initial data
  const { data: trade, ...queryState } = useQuery({
    queryKey,
    queryFn: () => getTradeById(tradeId),
    initialData: () => {
      // Fetch initial data from the cache
      return queryClient.getQueryData(queryKey);
    },
    keepPreviousData: true, // Keep old data while fetching new data
  });

  return {
    trade, // The current trade data
    ...queryState, // Other query state like isLoading, isError, etc.
  };
}

export default useTrade;
