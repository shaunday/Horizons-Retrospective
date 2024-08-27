import { useMutation, useQueryClient } from "@tanstack/react-query";
import { addTradeComposite } from "@services/tradesApiAccess";
import * as Constants from "@constants/journalConstants"; // Ensure the import path is correct

function useAddTrade() {
  const queryClient = useQueryClient();

  // Mutation to add a new trade
  const mutation = useMutation(
    () => addTradeComposite(), // No need for async/await here, React Query handles promises
    {
      onSuccess: (newTrade) => {
        // Update the cache with the new trade
        queryClient.setQueryData(
          [Constants.TRADE_KEY, newTrade.Id],
          (oldTrades = []) => {
            return [...oldTrades, newTrade]; // Append the new trade to the existing trades
          }
        );
      },
      onError: (error) => {
        console.error("Error adding trade:", error);
      },
    }
  );

  return {
    addTrade: mutation.mutate,
    status: mutation.status,
    error: mutation.error,
  };
}

export default useAddTrade;
