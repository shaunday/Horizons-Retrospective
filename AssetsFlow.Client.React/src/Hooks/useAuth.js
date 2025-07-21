import { useMutation, useQueryClient } from "@tanstack/react-query";
import { login, register, logout } from "@services/ApiRequests/authApiAccess";
import { useEffect } from "react";

const USER_KEY = ["authUser"];

export function useAuth() {
  const queryClient = useQueryClient();

  // Hydrate user from localStorage on app load
  useEffect(() => {
    const user = localStorage.getItem("authUser");
    if (user) {
      queryClient.setQueryData(USER_KEY, JSON.parse(user));
    }
  }, [queryClient]);

  const user = queryClient.getQueryData(USER_KEY);

  const loginMutation = useMutation({
    mutationFn: login,
    onSuccess: (data) => {
      queryClient.setQueryData(USER_KEY, data.user);
      localStorage.setItem("jwtToken", data.token);
      localStorage.setItem("authUser", JSON.stringify(data.user));
    },
  });

  const registerMutation = useMutation({
    mutationFn: register,
    onSuccess: (data) => {
      queryClient.setQueryData(USER_KEY, data.user);
      localStorage.setItem("jwtToken", data.token);
      localStorage.setItem("authUser", JSON.stringify(data.user));
    },
  });

  const handleLogout = () => {
    logout();
    queryClient.removeQueries(USER_KEY);
    localStorage.removeItem("jwtToken");
    localStorage.removeItem("authUser");
  };

  // Helper to get the token for API calls
  const getToken = () => localStorage.getItem("jwtToken");

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