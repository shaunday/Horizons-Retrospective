import { useQuery, useQueryClient } from "@tanstack/react-query";
import { tradeKeysFactory } from "@services/query-key-factory";
import { getTradeById } from "@services/ApiRequests/journalApiAccess";

export function useGetTradeById(tradeId) {
  const queryClient = useQueryClient();
  const queryKey = tradeKeysFactory.getTradeAndIdArrayKey(tradeId);
  const { data: trade, ...queryState } = useQuery({
    queryKey,
    queryFn: () => getTradeById(tradeId),
    initialData: () => queryClient.getQueryData(queryKey),
    keepPreviousData: true,
    refetchOnMount: false,
    refetchOnWindowFocus: false,
    throwOnError: true,
  });
  return { trade, ...queryState };
}

export default useGetTradeById;
