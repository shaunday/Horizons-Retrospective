import { useMutation, useQueryClient } from "@tanstack/react-query";
import { login, register, logout } from "@services/ApiRequests/authApiAccess";
import { useEffect } from "react";

const TOKEN_KEY = ["jwtToken"];
const USER_KEY = ["authUser"];

export function useAuth() {
  const queryClient = useQueryClient();

  // Hydrate token and user from localStorage on app load
  useEffect(() => {
    const token = localStorage.getItem("jwtToken");
    if (token) {
      queryClient.setQueryData(TOKEN_KEY, token);
    }
    const user = localStorage.getItem("authUser");
    if (user) {
      queryClient.setQueryData(USER_KEY, JSON.parse(user));
    }
  }, [queryClient]);

  const token = queryClient.getQueryData(TOKEN_KEY);
  const user = queryClient.getQueryData(USER_KEY);

  const loginMutation = useMutation({
    mutationFn: login,
    onSuccess: (data) => {
      queryClient.setQueryData(TOKEN_KEY, data.token);
      queryClient.setQueryData(USER_KEY, data.user);
      localStorage.setItem("jwtToken", data.token);
      localStorage.setItem("authUser", JSON.stringify(data.user));
    },
  });

  const registerMutation = useMutation({
    mutationFn: register,
    onSuccess: (data) => {
      queryClient.setQueryData(TOKEN_KEY, data.token);
      queryClient.setQueryData(USER_KEY, data.user);
      localStorage.setItem("jwtToken", data.token);
      localStorage.setItem("authUser", JSON.stringify(data.user));
    },
  });

  const handleLogout = () => {
    logout();
    queryClient.removeQueries(TOKEN_KEY);
    queryClient.removeQueries(USER_KEY);
    localStorage.removeItem("jwtToken");
    localStorage.removeItem("authUser");
  };

  return {
    user,
    token,
    login: loginMutation.mutateAsync,
    register: registerMutation.mutateAsync,
    logout: handleLogout,
    isLoggingIn: loginMutation.isLoading,
    isRegistering: registerMutation.isLoading,
    loginError: loginMutation.error,
    registerError: registerMutation.error,
  };
} 