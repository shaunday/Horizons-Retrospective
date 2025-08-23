import { useQuery } from "@tanstack/react-query";
import { getUserData } from "@services/ApiRequests/userApiAccess";
import { userDataKeysFactory } from "@services/query-key-factory";
import { parseUserData } from "./userDataParser";

export function useUserData({ enabled = true } = {}) {
  const key = userDataKeysFactory.getKeyForUserData();

  const userDataQuery = useQuery({
    queryKey: key,
    enabled,
    throwOnError: true,
    queryFn: async () => {
      const raw = await getUserData();
      const parsed = parseUserData(raw);

      // Normalize shape so consumers don’t care about backend inconsistencies
      return {
        ...parsed,
        filters: parsed?.filters ?? parsed?.availableFilters ?? [],
        symbols: parsed?.symbols ?? parsed?.symbolRestrictions ?? [],
      };
    },
    onSuccess: (parsedNormalized) => {
      console.log("[useUserData] parsed:", parsedNormalized);
    },
  });

  const data = userDataQuery.data ?? {};
  const filters = data.filters ?? data.availableFilters ?? [];
  const symbols = data.symbols ?? data.symbolRestrictions ?? [];

  return {
    isLoading: userDataQuery.isLoading,
    isError: userDataQuery.isError,
    userData: data,
    filters,
    symbols,
  };
}
