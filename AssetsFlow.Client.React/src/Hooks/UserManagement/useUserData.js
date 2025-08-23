import { useQuery, useQueryClient } from "@tanstack/react-query";
import { getUserData } from "@services/ApiRequests/userApiAccess";
import { userDataKeysFactory } from "@services/query-key-factory";
import { parseUserData } from "./userDataParser";

export function useUserData({ enabled = true } = {}) {
  const queryClient = useQueryClient();

  const userDataQuery = useQuery({
    queryKey: userDataKeysFactory.getKeyForUserData(),
    queryFn: getUserData,
    enabled,
    onSuccess: (data) => {
      const parsed = parseUserData(data);
      queryClient.setQueryData(
        userDataKeysFactory.getKeyForUserData(),
        parsed
      );
    }
  });

  return {
    userDataQuery,
    symbols: userDataQuery.data?.symbols ?? [],
    filters: userDataQuery.data?.filters ?? [],
    refetch: userDataQuery.refetch,
  };
}