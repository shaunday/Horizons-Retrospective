import { useMutation, useQuery } from "@tanstack/react-query";
import {
  login,
  loginAsDemo,
  register,
  logout as logoutApi,
} from "@services/ApiRequests/authApiAccess";
import { authStorage, USER_QUERY_KEY } from "@services/authStorage";

export function useAuth() {
  const { data: user } = useQuery({
    queryKey: USER_QUERY_KEY,
    queryFn: authStorage.getUser,
  });

  const loginMutation = useMutation({
    mutationFn: login,
    onSuccess: (data) => authStorage.setAuth(data.token, data.user),
  });

  const loginAsDemoMutation = useMutation({
    mutationFn: loginAsDemo,
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
    login: loginMutation.mutateAsync,
    loginAsDemo: loginAsDemoMutation.mutateAsync,
    register: registerMutation.mutateAsync,
    logout,
    isLoggingIn: loginMutation.isLoading,
    isLoggingInAsDemo: loginAsDemoMutation.isLoading,
    isRegistering: registerMutation.isLoading,
    loginError: loginMutation.error,
    loginAsDemoError: loginAsDemoMutation.error,
    registerError: registerMutation.error,
  };
}