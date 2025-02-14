import { useQueryClient } from "@tanstack/react-query";
import { produce } from "immer";
import { tradeKeysFactory } from "@services/query-key-factory";

export function useElementActivation(tradeComposite) {
  const queryClient = useQueryClient();

  const tryActivate = (element) => {
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
