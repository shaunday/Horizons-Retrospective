import { useQuery, useQueryClient } from "@tanstack/react-query";
import { tradeKeysFactory } from "@services/query-key-factory";
import { getTradeById } from "@services/tradesApiAccess";

function useTrade(tradeId) {
  const queryClient = useQueryClient();

  const queryKey = tradeKeysFactory.tradeByIdKey(tradeId);

  const { data: trade, ...queryState } = useQuery({
    queryKey,
    queryFn: () => getTradeById(tradeId),
    initialData: () => queryClient.getQueryData(queryKey),
    keepPreviousData: true,
    refetchOnMount: false,
    refetchOnWindowFocus: false,
  });

  return { trade, ...queryState };
}

export default useTrade;
