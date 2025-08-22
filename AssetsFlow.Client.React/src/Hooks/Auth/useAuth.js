import { useMutation, useQuery } from "@tanstack/react-query";
import {
  login,
  loginAsDemo,
  register,
  logout as logoutApi,
} from "@services/ApiRequests/userApiAccess";
import { authStorage, USER_QUERY_KEY } from "@services/authStorage";

export function useAuth() {
  const { data: user } = useQuery({
    queryKey: USER_QUERY_KEY,
    queryFn: authStorage.getUser,
  });

  const loginMutation = useMutation({
    mutationFn: ({ isDemo, credentials }) =>
      isDemo ? loginAsDemo() : login(credentials),
    onSuccess: (data) => authStorage.setAuth(data.token, data.user),
  });

  const registerMutation = useMutation({
    mutationFn: register,
    onSuccess: (data) => authStorage.setAuth(data.token, data.user),
  });

  const logout = () => {
    authStorage.clearAll();
    logoutApi();
  };

  const getToken = () => authStorage.getToken();

  return {
    user,
    getToken,
    login: (credentials) => loginMutation.mutateAsync({ isDemo: false, credentials }),
    loginAsDemo: () => loginMutation.mutateAsync({ isDemo: true }),
    register: registerMutation.mutateAsync,
    logout,
    isLoggingIn: loginMutation.isLoading,
    isRegistering: registerMutation.isLoading,
    loginError: loginMutation.error,
    registerError: registerMutation.error,
  };
}