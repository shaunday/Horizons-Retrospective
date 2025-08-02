import { useQuery, useQueryClient } from "@tanstack/react-query";
import { getAllTrades } from "@services/ApiRequests/journalApiAccess";
import { tradeKeysFactory } from "@services/query-key-factory";

export function useFetchAndCacheTrades({ enabled = true } = {}) {
  const queryClient = useQueryClient();

  const getAndCacheTrades = async () => {
    const fetchedTrades = await getAllTrades();
    const tradeIds = fetchedTrades.map((trade) => trade.id);
    queryClient.setQueryData(tradeKeysFactory.getKeyForAllTradesByIds(), tradeIds);
    fetchedTrades.forEach((trade) => {
      queryClient.setQueryData(tradeKeysFactory.getTradeAndIdArrayKey(trade.id), trade);
    });
    return fetchedTrades;
  };

  const tradesQuery = useQuery({
    queryKey: tradeKeysFactory.getKeyForAllTrades(),
    queryFn: getAndCacheTrades,
    throwOnError: true,
    enabled,
  });

  const prefetchTrades = async () => {
    queryClient.prefetchQuery({
      queryKey: tradeKeysFactory.getKeyForAllTrades(),
      queryFn: getAndCacheTrades,
    });
  };

  return { 
    isLoading: tradesQuery.isLoading, 
    isError: tradesQuery.isError, 
    trades: tradesQuery.data, 
    prefetchTrades 
  };
}