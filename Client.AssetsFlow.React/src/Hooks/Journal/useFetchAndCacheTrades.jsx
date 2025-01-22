import { useQuery, useQueryClient } from "@tanstack/react-query";
import { getAllTrades } from "@services/tradesApiAccess";
import { tradeKeysFactory } from "@services/query-key-factory";

export function useFetchAndCacheTrades() {
  const queryClient = useQueryClient();

  const getAndCacheTrades = async () => {
    const fetchedTrades = await getAllTrades();
    
    // Cache the trades here
    const tradeIds = fetchedTrades.map((trade) => trade["id"]);
    queryClient.setQueryData(tradeKeysFactory.tradeIdsKey, tradeIds);
  
    fetchedTrades.forEach((trade) => {
      const tradeId = trade["id"];
      queryClient.setQueryData(tradeKeysFactory.tradeByIdKey(tradeId), trade);
  });
     
    return fetchedTrades;
  };

  const tradesQuery = useQuery({
    queryKey: tradeKeysFactory.tradesKey,
    queryFn: getAndCacheTrades,
    onError: (error) => console.log("error in tradesQ:", error),
    onSuccess: (data) => console.log("Success in usequery:", data),
  });

  const prefetchTrades = async () => {
    queryClient.prefetchQuery({
      queryKey: tradeKeysFactory.tradesKey,
      queryFn: getAndCacheTrades,
      onError: (error) => console.log("error in tradesQ:", error),
     onSuccess: (data) => console.log("Success in prefetch:", data),
    });
  };

  return { 
    isLoading: tradesQuery.isLoading, 
    isError: tradesQuery.isError, 
    trades: tradesQuery.data, 
    prefetchTrades 
  };
}
