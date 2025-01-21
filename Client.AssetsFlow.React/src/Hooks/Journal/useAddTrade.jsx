import { useMutation, useQueryClient } from "@tanstack/react-query";
import { addTradeComposite } from "@services/tradesApiAccess";
import { tradeKeysFactory } from "@services/query-key-factory";

export function useAddTrade() {
  const queryClient = useQueryClient();

  const {
    mutate: addTrade,
    isLoading: isAddingTrade,
    isError: isAddTradeError,
  } = useMutation({
    mutationFn: async () => {
      const newTrade = await addTradeComposite();
      return newTrade;
    },
    onSuccess: (newTrade) => {
      const tradeId = newTrade["id"];
      queryClient.setQueryData(
        tradeKeysFactory.tradeByIdKey(tradeId),
        newTrade
      );

      const currentTradeIds =
        queryClient.getQueryData(tradeKeysFactory.tradeIdsKey) || [];
      queryClient.setQueryData(tradeKeysFactory.tradeIdsKey, [
        ...currentTradeIds,
        tradeId,
      ]);
    },
  });

  return { addTrade, isAddingTrade, isAddTradeError };
}
