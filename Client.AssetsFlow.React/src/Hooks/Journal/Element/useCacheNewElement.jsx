import { useQueryClient } from "@tanstack/react-query";
import { produce } from "immer";
import { tradeKeysFactory } from "@services/query-key-factory";

export function useCacheNewElement(tradeComposite) {
  const queryClient = useQueryClient();

  const onElementUpdate = (newElement) => {
    queryClient.setQueryData(
      tradeKeysFactory.tradeByIdKey(tradeComposite.id),
      (oldTradeComposite) =>
        produce(oldTradeComposite, (draft) => {
          draft.tradeElements.push(newElement);
        })
    );
  };

  return { onElementUpdate };
}
