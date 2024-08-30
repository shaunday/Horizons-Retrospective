import { queryClient } from '@tanstack/react-query';
import * as Constants from '@constants/constants';

function saveTrade(clientId, data) {
  const updatedData = {
    ...data,
    [Constants.TRADE_CLIENTID_PROP_STRING]: clientId,
  };

  // Update the cache with the new data
  queryClient.setQueryData([Constants.TRADE_KEY, clientId], updatedData);
}

function getTradeQueryConfig(clientId, trade = null) {
  return {
    queryKey: [Constants.TRADE_KEY, clientId],
    queryFn: async () => await getTradeById(clientId),
    onSuccess: (data) => saveTrade(clientId, data),
    keepPreviousData: true,
    ...(trade !== null && { initialData: trade }), // Set initialData only if trade is not null
  };
}

export { TradeQuery };
