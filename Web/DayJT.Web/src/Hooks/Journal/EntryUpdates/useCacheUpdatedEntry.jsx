import { useQueryClient } from "@tanstack/react-query";
import { produce } from "immer";
import * as Constants from "@constants/constants";

export function useCacheUpdatedEntry(tradeComposite) {
  const queryClient = useQueryClient();
  const clientIdValue = tradeComposite[Constants.TRADE_CLIENT_ID_PROPERTY];

  const onEntryUpdate = (data) => {
    const { updatedEntry } = data;
    queryClient.setQueryData(
      [Constants.TRADE_KEY, clientIdValue],
      (oldTradeComposite) =>
        produce(oldTradeComposite, (draft) => {
          const tradeElements = draft[Constants.TRADE_ELEMENTS_STRING];
          for (const tradeElement of tradeElements) {
            const entryIndex = tradeElement.entries.findIndex(
              (entry) => entry.id === updatedEntry.id
            );
            if (entryIndex !== -1) {
              // Update the specific Entry
              tradeElement.entries[entryIndex] = updatedEntry;
              break;
            }
          }
        })
    );
  };

  return { onEntryUpdate };
}
