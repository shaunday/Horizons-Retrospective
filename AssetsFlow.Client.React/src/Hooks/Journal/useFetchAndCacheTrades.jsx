import { useQuery, useQueryClient } from "@tanstack/react-query";
import { getAllTrades } from "@services/ApiRequests/journalApiAccess";
import { tradeKeysFactory } from "@services/query-key-factory";

export function useFetchAndCacheTrades() {
  const queryClient = useQueryClient();

  const getAndCacheTrades = async () => {
    const fetchedTrades = await getAllTrades();
    
    // Cache the trades here
    const tradeIds = fetchedTrades.map((trade) => trade.id);
    queryClient.setQueryData(tradeKeysFactory.tradeIdsKey, tradeIds);
  
    fetchedTrades.forEach((trade) => {
      const tradeId = trade.id;
      queryClient.setQueryData(tradeKeysFactory.tradeAndIdArrayKey(tradeId), trade);
  });
     
    return fetchedTrades;
  };

  const tradesQuery = useQuery({
    queryKey: tradeKeysFactory.tradesKey,
    queryFn: getAndCacheTrades,
    throwOnError: true,
  });

  const prefetchTrades = async () => {
    queryClient.prefetchQuery({
      queryKey: tradeKeysFactory.tradesKey,
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
