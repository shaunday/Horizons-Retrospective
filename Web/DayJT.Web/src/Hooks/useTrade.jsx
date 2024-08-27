import { useQuery, useQueryClient } from "@tanstack/react-query";
import * as Constants from "@constants/journalConstants"; // Ensure the import path is correct
import { getTradeById } from "@services/tradesApiAccess";

function useTrade(tradeId) {
  const queryClient = useQueryClient();

  // Fetch trade by ID
  const fetchTradeById = async (id) => {
    const response = await getTradeById(id);
    return response.data;
  };

  // Query to fetch trade data
  const {
    data: trade,
    isLoading,
    isError,
  } = useQuery([Constants.TRADE_KEY, tradeId], () => fetchTradeById(tradeId), {
    onSuccess: (data) => {
      queryClient.setQueryData([Constants.TRADE_KEY, data.Id], data);
    },
    keepPreviousData: true,
  });

  return {
    trade,
    isLoading,
    isError,
  };
}

export default useTrade;
