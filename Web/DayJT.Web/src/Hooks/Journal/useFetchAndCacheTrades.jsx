import { useQuery, useQueryClient } from "@tanstack/react-query";
import { getAllTrades } from "@services/tradesApiAccess"; // Function to fetch all trades
import * as Constants from "@constants/journalConstants";

export function useFetchAndCacheTrades() {
  const queryClient = useQueryClient();

  // Fetch all trades
  const {
    data: trades,
    isLoading,
    isError,
  } = useQuery({
    queryKey: [Constants.TRADES_ARRAY_KEY],
    queryFn: getAllTrades,
    onSuccess: (fetchedTrades) => {
      // Extract trade IDs and cache them
      const tradeIds = fetchedTrades.map((trade) => trade["ID"]);

      // Cache all trade IDs
      queryClient.setQueryData([Constants.TRADE_IDS_ARRAY_KEY], tradeIds);

      // Cache each trade individually
      fetchedTrades.forEach((trade) => {
        const tradeId = trade["ID"];
        queryClient.setQueryData([Constants.TRADE_KEY, tradeId], trade);
      });
    },
  });

  return {
    isLoading,
    isError,
    trades, // Optionally return the fetched trades if needed
  };
}
