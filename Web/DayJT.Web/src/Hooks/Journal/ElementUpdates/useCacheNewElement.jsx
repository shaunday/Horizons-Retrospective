import { useQueryClient } from "@tanstack/react-query";
import { produce } from "immer";
import * as Constants from "@constants/constants";

export function useCacheNewElement(tradeComposite) {
  const queryClient = useQueryClient();
  const clientIdValue = tradeComposite[Constants.TRADE_CLIENT_ID_PROPERTY];

  const onElementUpdate = (newElement) => {
    queryClient.setQueryData(
      [Constants.TRADE_KEY, clientIdValue],
      (oldTradeComposite) =>
        produce(oldTradeComposite, (draft) => {
          draft[Constants.TRADE_ELEMENTS_STRING].push(newElement);
        })
    );
  };

  return { onElementUpdate };
}
