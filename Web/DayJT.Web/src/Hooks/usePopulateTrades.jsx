import { useQueries } from "@tanstack/react-query";
import * as Constants from "@constants/journalConstants"; // Adjust path as needed
import { getTradeById } from "@services/tradesApiAccess"; // Adjust path as needed

function usePopulateTrades(trades) {
  const queries = useQueries(
    trades.map((trade) => ({
      queryKey: [Constants.TRADE_KEY, trade.Id],
      queryFn: () => getTradeById(tradeId).then((res) => res.data),
      initialData: trade,
      enabled: !!tradeId, // Only run query if tradeId is available
    }))
  );

  return {
    trades: queries.map((query) => query.data),
    isLoading: queries.some((query) => query.isLoading),
    isError: queries.some((query) => query.isError),
  };
}

export default usePopulateTrades;
