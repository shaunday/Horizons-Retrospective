import { useQueryClient } from "@tanstack/react-query";
import { authStorage } from "@services/authStorage";
import { tradeKeysFactory, userDataKeysFactory } from "@services/query-key-factory";

export function useClearTradesCache() {
    const queryClient = useQueryClient();

    return async function clear() {
        const userId = authStorage.getUserId?.();
        if (!userId) return;

        // Invalidate all trades list
        const allTradesKey = tradeKeysFactory.getKeyForAllTrades();
        queryClient.invalidateQueries({
            queryKey: allTradesKey,
            exact: true,
            refetchActive: true,
            refetchInactive: false,
        });

        // Invalidate user data cache
        const userDataKey = userDataKeysFactory.getKeyForUserData();
        queryClient.invalidateQueries({
            queryKey: userDataKey,
            exact: false,
        });
    };
}
