import { useQueryClient } from "@tanstack/react-query";
import { produce } from "immer";
import { tradeKeysFactory } from "@services/query-key-factory";

export function useUpdateTradeCacheData(tradeId) {
  const queryClient = useQueryClient();

  const setNewData = (dataIdentifier, newData) => {

    queryClient.setQueryData(
      tradeKeysFactory.tradeAndIdArrayKey(tradeId),
      (oldTradeComposite) =>
        produce(oldTradeComposite, (draft) => {
            draft[dataIdentifier] = newData;
          }));
  };

  return { setNewData };
}
