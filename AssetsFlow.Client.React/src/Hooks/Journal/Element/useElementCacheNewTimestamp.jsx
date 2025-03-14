import { useQueryClient } from "@tanstack/react-query";
import { produce } from "immer";
import * as Constants from "@constants/journalConstants";
import { tradeKeysFactory } from "@services/query-key-factory";

export function useElementCacheNewTimestamp(tradeElement) {
  const queryClient = useQueryClient();
  const tradeId = tradeElement[Constants.ELEMENT_COMPOSITEFK_STING];

  const setNewTimeStamp = (newStamp) => {

    queryClient.setQueryData(
      tradeKeysFactory.tradeAndIdArrayKey(tradeId),
      (oldTradeComposite) =>
        produce(oldTradeComposite, (draft) => {
          const tradeElements = draft[Constants.TRADE_ELEMENTS_STRING];
          for (const element of tradeElements) {
            if (element.id === tradeElement.id) {
              element[Constants.ELEMENT_TIMESTAMP_STING] = newStamp;
            }
          }
        })
    );
  };

  return { setNewTimeStamp };
}
