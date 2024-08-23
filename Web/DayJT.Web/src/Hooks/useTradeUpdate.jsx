import { useQueryClient } from '@tanstack/react-query';
import { produce } from 'immer';
import * as Constants from '@constants/constants';

export function useTradeUpdate(tradeComposite) {
    const queryClient = useQueryClient();

    const onElementUpdate = (data) => {
        const { updatedEntry, newSummary } = data;
        const clientIdValue = tradeComposite[Constants.TRADE_CLIENTID_KEY];

        queryClient.setQueryData([Constants.TRADES_KEY, clientIdValue], (oldTradeComposite) =>
            produce(oldTradeComposite, draft => {
                const tradeElements = draft[Constants.TRADE_ELEMENTS_KEY];
                for (const tradeElement of tradeElements) {
                    const entryIndex = tradeElement.entries.findIndex(entry => entry.id === updatedEntry.id);
                    if (entryIndex !== -1) {
                        // Update the specific Entry
                        tradeElement.entries[entryIndex] = updatedEntry;
                        break;
                    }
                }
            })
        );

        // Update summary
        queryClient.setQueryData([Constants.TRADECOMPOSITE_SUMMARY_KEY, tradeComposite.Id], newSummary);
    };

    return { onElementUpdate };
}