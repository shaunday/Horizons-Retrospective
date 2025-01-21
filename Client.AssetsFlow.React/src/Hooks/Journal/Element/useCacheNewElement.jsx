import { useQueryClient } from "@tanstack/react-query";
import { produce } from "immer";
import { tradeKeysFactory } from "@services/query-key-factory";

export function useCacheNewElement(tradeComposite) {
  const queryClient = useQueryClient();
  const tradeId = tradeComposite["id"];

  const onElementUpdate = (newElement) => {
    queryClient.setQueryData(
      tradeKeysFactory.tradeByIdKey(tradeId),
      (oldTradeComposite) =>
        produce(oldTradeComposite, (draft) => {
          draft.tradeElements.push(newElement);
        })
    );
  };

  return { onElementUpdate };
}
