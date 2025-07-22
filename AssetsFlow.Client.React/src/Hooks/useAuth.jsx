import { useMutation, useQuery } from "@tanstack/react-query";
import { login, register, logout } from "@services/ApiRequests/authApiAccess";
import { authStorage, USER_QUERY_KEY } from "@services/ApiRequests/authStorage";

export function useAuth() {
  const { data: user } = useQuery(USER_QUERY_KEY, authStorage.getUser);

  const loginMutation = useMutation({
    mutationFn: login,
    onSuccess: (data) => authStorage.setAuth(data.token, data.user),
  });

  const registerMutation = useMutation({
    mutationFn: register,
    onSuccess: (data) => authStorage.setAuth(data.token, data.user),
  });

  const handleLogout = () => {
    logout();
    authStorage.clearAll();
  };

  const getToken = () => authStorage.getToken();

  return {
    user,
    getToken,
    login: loginMutation.mutateAsync,
    register: registerMutation.mutateAsync,
    logout: handleLogout,
    isLoggingIn: loginMutation.isLoading,
    isRegistering: registerMutation.isLoading,
    loginError: loginMutation.error,
    registerError: registerMutation.error,
  };
}
