import { useQuery, useQueryClient } from "@tanstack/react-query";
import { getAllTrades } from "@services/ApiRequests/journalApiAccess";
import { tradeKeysFactory } from "@services/query-key-factory";

export function useFetchAndCacheTrades() {
  const queryClient = useQueryClient();

  const getAndCacheTrades = async () => {
    const fetchedTrades = await getAllTrades();
    const tradeIds = fetchedTrades.map((trade) => trade.id);
    queryClient.setQueryData(tradeKeysFactory.getTradeIdsKey(), tradeIds);
    fetchedTrades.forEach((trade) => {
      queryClient.setQueryData(tradeKeysFactory.getTradeAndIdArrayKey(trade.id), trade);
    });
    return fetchedTrades;
  };

  const tradesQuery = useQuery({
    queryKey: tradeKeysFactory.getTradesKey(),
    queryFn: getAndCacheTrades,
    throwOnError: true,
  });

  const prefetchTrades = async () => {
    queryClient.prefetchQuery({
      queryKey: tradeKeysFactory.getTradesKey(),
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
