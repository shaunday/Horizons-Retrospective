import { useQuery, useQueryClient } from "@tanstack/react-query";
import { getAllTrades } from "@services/tradesApiAccess";
import { tradeKeysFactory } from "@services/query-key-factory";

export function useFetchAndCacheTrades() {
  const queryClient = useQueryClient();

  const cacheTrades = (fetchedTrades) => {
    const tradeIds = fetchedTrades.map((trade) => trade["ID"]);
    queryClient.setQueryData(tradeKeysFactory.tradeIdsKey, tradeIds);

    fetchedTrades.forEach((trade) => {
      const tradeId = trade["ID"];
      queryClient.setQueryData(tradeKeysFactory.tradeByIdKey(tradeId), trade);
    });
  };

  const tradesQuery = useQuery({
    queryKey: tradeKeysFactory.tradesKey,
    queryFn: getAllTrades,
    onSuccess: cacheTrades,
  });

  const prefetchTrades = async () => {
    await queryClient.prefetchQuery({
      queryKey: tradeKeysFactory.tradesKey,
      queryFn: getAllTrades,
      onSuccess: cacheTrades,
    });
  };

  return { tradesQuery, prefetchTrades };
}
