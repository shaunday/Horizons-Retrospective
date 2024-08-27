import { useQuery, useQueryClient } from "@tanstack/react-query";
import * as Constants from "@constants/journalConstants"; // Ensure this path is correct
import { getAllTrades } from "@services/tradesApiAccess"; // Ensure this path is correct

function useAllTrades() {
  const queryClient = useQueryClient();

  // Fetch all trades
  const {
    data: trades = [],
    isLoading,
    isError,
  } = useQuery(
    [Constants.ALL_TRADES_KEY],
    getAllTrades, // Fetches all trades
    {
      onSuccess: (trades) => {
        trades.forEach((trade) => {
          // Set up individual queries for each trade
          queryClient.setQueryData([Constants.TRADE_KEY, trade.Id], trade);
        });
      },
    }
  );

  return {
    trades,
    isLoading,
    isError,
  };
}

export default useAllTrades;
