import { useQueryClient } from "@tanstack/react-query";
import { produce } from "immer";
import * as Constants from "@constants/journalConstants";
import { tradeKeysFactory } from "@services/query-key-factory";

export function useCacheElementActivation(tradeElement) {
  const queryClient = useQueryClient();

  const setNewActiveState = (newState) => {
    const tradeId = tradeElement[Constants.ELEMENT_COMPOSITEFK_STING];

    queryClient.setQueryData(
      tradeKeysFactory.tradeAndIdArrayKey(tradeId),
      (oldTradeComposite) =>
        produce(oldTradeComposite, (draft) => {
          const tradeElements = draft[Constants.TRADE_ELEMENTS_STRING];
          for (const element of tradeElements) {
            if (element.id === tradeElement.id) {
              element.isActive = newState;
            }
          }
        })
    );
  };

  return { setNewActiveState };
}
