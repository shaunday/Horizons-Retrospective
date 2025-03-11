import { useMutation, useQueryClient } from "@tanstack/react-query";
import { addTradeComposite } from "@services/ApiRequests/journalApiAccess";
import { tradeKeysFactory } from "@services/query-key-factory";

export function useAddTrade() {
  const queryClient = useQueryClient();

  const {
    mutate: addTrade,
    isLoading: isAddingTrade,
    isError: isAddTradeError,
  } = useMutation({
    mutationFn: () => {
      const newTrade = addTradeComposite();
      return newTrade;
    },
    onSuccess: (newTrade) => {
      const tradeAndIdArray = tradeKeysFactory.tradeAndIdArrayKey(newTrade.id)
      queryClient.setQueryData(tradeAndIdArray, newTrade);

      const currentTradeIds = queryClient.getQueryData(tradeKeysFactory.tradeIdsKey) || [];
      queryClient.setQueryData(tradeKeysFactory.tradeIdsKey, [
        ...currentTradeIds,
        newTrade.id,
      ]);
    },
  });

  return { addTrade, isAddingTrade, isAddTradeError }; //todo use those ?
}
