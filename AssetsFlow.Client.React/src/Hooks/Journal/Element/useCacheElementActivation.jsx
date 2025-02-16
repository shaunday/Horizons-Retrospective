import { useQueryClient } from "@tanstack/react-query";
import { produce } from "immer";
import * as Constants from "@constants/journalConstants";
import { tradeKeysFactory } from "@services/query-key-factory";

export function useCacheElementActivation(tradeElement) {
  const queryClient = useQueryClient();

  const setNewActiveState = (newState) => {
    const tradeId = tradeElement[Constants.ELEMENT_COMPOSITEFK_STING].id;
    
    queryClient.setQueryData(
      tradeKeysFactory.tradeAndIdArrayKey(tradeId),
      (oldTradeComposite) =>
        produce(oldTradeComposite, (draft) => {
          const element = draft.tradeElements.find(
            (el) => el.id === tradeElement.id
          );
          if (element) {
            element.isActive = newState;
          }
        })
    );
  };

  return { setNewActiveState };
}