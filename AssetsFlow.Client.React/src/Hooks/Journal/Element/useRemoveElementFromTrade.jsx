import React from "react";
import { useQueryClient } from "@tanstack/react-query";
import { produce } from "immer";
import { tradeKeysFactory } from "@services/query-key-factory";

export function useRemoveElementFromTrade(tradeId) {
  const queryClient = useQueryClient();
  const tradeAndIdArray = tradeKeysFactory.tradeAndIdArrayKey(tradeId)

  const removeElement = (elementId) => {
    queryClient.setQueryData(tradeAndIdArray, (oldTrade) => {
      if (!oldTrade) return oldTrade;
      return produce(oldTrade, (draft) => {
        draft.tradeElements = draft.tradeElements.filter(ele => ele.id !== elementId);
      });
    });
  };

  return { removeElement };
}
