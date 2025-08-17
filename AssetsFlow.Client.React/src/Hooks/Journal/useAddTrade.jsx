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
    mutationFn: () => addTradeComposite(),
    onSuccess: (newTrade) => {
      // 1) Cache the new trade individually
      queryClient.setQueryData(tradeKeysFactory.getKeyForTradeById(newTrade.id), newTrade);

      // 2) Update the "AllTrades" array in cache so UI sees it immediately
      queryClient.setQueryData(tradeKeysFactory.getKeyForAllTrades(), (oldTrades = []) => [
        ...oldTrades,
        newTrade,
      ]);

      // 3) Optionally mark the newly added trade for highlighting in UI
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
