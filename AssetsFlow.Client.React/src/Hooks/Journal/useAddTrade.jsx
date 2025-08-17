import { useMutation, useQueryClient } from "@tanstack/react-query";
import { addTradeComposite } from "@services/ApiRequests/journalApiAccess";
import { tradeKeysFactory } from "@services/query-key-factory";
import { useState } from "react";

export function useAddTrade() {
  const queryClient = useQueryClient();
  const [newlyAddedTradeId, setNewlyAddedTradeId] = useState(null);

  const {
    mutate: addTrade,
    isLoading: isAddingTrade,
    isError: isAddTradeError,
  } = useMutation({
    mutationFn: () => {
      return addTradeComposite();
    },
    onSuccess: (newTrade) => {
      const tradeAndIdArray = tradeKeysFactory.getKeyForTradeById(newTrade.id);
      queryClient.setQueryData(tradeAndIdArray, newTrade);
      const currentTradeIds = queryClient.getQueryData(tradeKeysFactory.getTradeIdsKey()) || [];
      queryClient.setQueryData(tradeKeysFactory.getTradeIdsKey(), [
        ...currentTradeIds,
        newTrade.id,
      ]);
      setNewlyAddedTradeId(newTrade.id);
    },
  });

  return {
    addTrade,
    isAddingTrade,
    isAddTradeError,
    newlyAddedTradeId,
  };
}
