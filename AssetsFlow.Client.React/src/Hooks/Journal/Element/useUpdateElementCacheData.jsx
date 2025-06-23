import { useQueryClient } from "@tanstack/react-query";
import { produce } from "immer";
import { tradeKeysFactory } from "@services/query-key-factory";
import * as Constants from "@constants/journalConstants";

export function useUpdateElementCacheData(tradeId, tradeElementId) {
  const queryClient = useQueryClient();

  const updateElementProp = (dataIdentifier, newData) => {
    queryClient.setQueryData(
      tradeKeysFactory.tradeAndIdArrayKey(tradeId),
      produce((oldTradeComposite) => {
        const tradeElements = oldTradeComposite[Constants.TRADE_ELEMENTS_STRING];
        const eleIndex = tradeElements.findIndex(
          (ele) => ele.id === tradeElementId
        );
        if (eleIndex !== -1) {
          tradeElements[eleIndex][dataIdentifier] = newData;
        }
      })
    );
  };

  return updateElementProp;
}