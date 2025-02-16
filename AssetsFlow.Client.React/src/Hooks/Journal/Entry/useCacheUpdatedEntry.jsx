import { useQueryClient } from "@tanstack/react-query";
import { produce } from "immer";
import { tradeKeysFactory } from "@services/query-key-factory";

export function useCacheUpdatedEntry(tradeComposite) {
  const queryClient = useQueryClient();

  const onEntryUpdate = (data) => {
    const { updatedEntry } = data;
    queryClient.setQueryData(
      tradeKeysFactory.tradeAndIdArrayKey(tradeComposite.id),
      (oldTradeComposite) =>
        produce(oldTradeComposite, (draft) => {
          const tradeElements = draft.tradeElements;
          for (const tradeElement of tradeElements) {
            const entryIndex = tradeElement.entries.findIndex(
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
