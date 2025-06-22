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

      // Track the newly added trade ID
      setNewlyAddedTradeId(newTrade.id);
    },
  });

  return { 
    addTrade, 
    isAddingTrade, 
    isAddTradeError, 
    newlyAddedTradeId
  };
}
