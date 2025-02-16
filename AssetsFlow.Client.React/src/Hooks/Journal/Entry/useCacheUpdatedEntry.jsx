import { useQueryClient } from "@tanstack/react-query";
import { produce } from "immer";
import { tradeKeysFactory } from "@services/query-key-factory";
import * as Constants from "@constants/journalConstants";

export function useCacheUpdatedEntry(tradeComposite) {
  const queryClient = useQueryClient();

  const onEntryUpdate = (data) => {
    const { updatedEntry } = data;
    queryClient.setQueryData(
      tradeKeysFactory.tradeAndIdArrayKey(tradeComposite.id),
      (oldTradeComposite) =>
        produce(oldTradeComposite, (draft) => {
          const tradeElements = draft[Constants.TRADE_ELEMENTS_STRING];
          for (const tradeElement of tradeElements) {
            const entryIndex = tradeElement[Constants.TRADE_ENTRIES_STRING].findIndex(
              (entry) => entry.id === updatedEntry.id
            );
            if (entryIndex !== -1) {
              tradeElement.entries[entryIndex] = updatedEntry;
              break;
            }
          }
        })
    );
  };

  return { onEntryUpdate };
}
