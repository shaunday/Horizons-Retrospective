import { useMutation, useQuery } from "@tanstack/react-query";
import { login, register, logout as logoutApi } from "@services/ApiRequests/userApiAccess";
import { authStorage, USER_QUERY_KEY } from "@services/authStorage";
import { useClearTradesCache } from "@hooks/UserManagement/useClearTradesCache";
import { useUserData } from "@hooks/UserManagement/useUserData";

export function useAuth() {
  const { data: user } = useQuery({
    queryKey: USER_QUERY_KEY,
    queryFn: authStorage.getUser,
  });

  const { getUserData, setUserDataFromApi } = useUserData();
  const loginMutation = useMutation({
    mutationFn: (credentials) => login(credentials),
    onSuccess: async (data) => {
      authStorage.setAuth(data.token, data.user);
      console.log("Logged in user:", data.user);

      const fetchedData = await getUserData();
      setUserDataFromApi(fetchedData);
    }
  });

  const registerMutation = useMutation({
    mutationFn: register,
    onSuccess: (data) => authStorage.setAuth(data.token, data.user),
  });

  const clearTradesCache = useClearTradesCache();
  const logout = () => {
    authStorage.clearAll();
    clearTradesCache();
    logoutApi();
  };

  const getToken = () => authStorage.getToken();

  return {
    user,
    getToken,
    login: (credentials) => loginMutation.mutateAsync(credentials),
    loginAsDemo: () => loginMutation.mutateAsync(), // send empty object internally
    register: registerMutation.mutateAsync,
    logout,
    isLoggingIn: loginMutation.isLoading,
    isRegistering: registerMutation.isLoading,
    loginError: loginMutation.error,
    registerError: registerMutation.error,
  };
}
