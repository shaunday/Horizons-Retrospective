// useUserData.js
import { useQuery, useQueryClient } from "@tanstack/react-query";
import { getUserData } from "@services/ApiRequests/userApiAccess";
import { userDataKeysFactory } from "@services/query-key-factory";
import { parseUserData } from "./userDataParser";
import { authStorage } from "@services/authStorage";

export function useUserData({ enabled } = {}) {
    const queryClient = useQueryClient();
    const isAuthenticated = !!authStorage.getToken();

    const query = useQuery({
        queryKey: userDataKeysFactory.getKeyForUserData(),
        queryFn: getUserData,
        enabled: enabled ?? isAuthenticated, // default: only run if authed
    });

    const setUserDataFromApi = (data) => {
        queryClient.setQueryData(
            userDataKeysFactory.getKeyForUserData(),
            parseUserData(data)
        );
    };

    const symbolRestrictions = query.data?.symbols ?? [];

    return {
        ...query,
        setUserDataFromApi,
        symbolRestrictions,
    };
}
