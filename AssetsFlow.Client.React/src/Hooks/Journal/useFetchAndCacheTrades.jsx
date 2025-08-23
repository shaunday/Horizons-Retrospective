import { useQuery, useQueryClient } from "@tanstack/react-query";
import { getAllTrades } from "@services/ApiRequests/journalApiAccess";
import { tradeKeysFactory } from "@services/query-key-factory";

export function useFetchAndCacheTrades({ enabled = true } = {}) {
  const queryClient = useQueryClient();

  const getAndCacheTrades = async () => {
    const fetchedTrades = await getAllTrades();
    fetchedTrades.forEach((trade) => {
      queryClient.setQueryData(tradeKeysFactory.getKeyForTradeById(trade.id), trade);
    });
    return fetchedTrades;
  };

  const tradesQuery = useQuery({
    queryKey: tradeKeysFactory.getKeyForAllTrades(),
    queryFn: getAndCacheTrades,
    onSuccess: (data) => {
      console.log(`Fetched and cached ${data.length} trades`);
    },
    throwOnError: true,
    enabled,
  });

  return {
    isLoading: tradesQuery.isLoading,
    isError: tradesQuery.isError,
    trades: tradesQuery.data,
  };
}
