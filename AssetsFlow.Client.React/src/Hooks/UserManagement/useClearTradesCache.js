import { useQueryClient } from "@tanstack/react-query";
import { authStorage } from "@services/authStorage";
import { tradeKeysFactory } from "@services/query-key-factory";

export function useClearTradesCache() {
    const queryClient = useQueryClient();

    return async function clear() {
        const userId = authStorage.getUserId?.();
        if (!userId) return;

        const allTradesKey = tradeKeysFactory.getKeyForAllTrades();

        // Only invalidate the main list — per-trade cache will update automatically
        queryClient.invalidateQueries({
            queryKey: allTradesKey,
            exact: true,  // exact match
            refetchActive: true, // refetch active queries immediately
            refetchInactive: false,
        });
    };
}
