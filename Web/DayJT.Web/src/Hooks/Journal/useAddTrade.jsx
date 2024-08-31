import { useMutation, useQueryClient } from "@tanstack/react-query";
import { addTradeComposite } from "@services/tradesApiAccess"; // Function to add a trade
import * as Constants from "@constants/journalConstants";

export function useAddTrade() {
  const queryClient = useQueryClient();

  // Mutation to add a trade
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
      const tradeId = newTrade["ID"];

      // Update the trade cache
      queryClient.setQueryData([Constants.TRADE_KEY, tradeId], newTrade);

      // Update the trade IDs cache
      const currentTradeIds =
        queryClient.getQueryData([Constants.TRADE_IDS_ARRAY_KEY]) || [];
      queryClient.setQueryData(
        [Constants.TRADE_IDS_ARRAY_KEY],
        [...currentTradeIds, tradeId]
      );
    },
    onError: () => {
      // Handle error if needed
    },
  });

  return {
    addTrade,
    isAddingTrade,
    isAddTradeError,
  };
}
