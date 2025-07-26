import { queryClient } from "@src/main.jsx";

export const USER_QUERY_KEY = ["user"];
const JWT_KEY = "jwtToken";
const USER_STORAGE_KEY = "authUser";

export const authStorage = {
  getToken() {
    return localStorage.getItem(JWT_KEY);
  },

  getUser() {
    return (
      queryClient.getQueryData(USER_QUERY_KEY) ??
      JSON.parse(localStorage.getItem(USER_STORAGE_KEY) || "null")
    );
  },

  getUserId() {
    const user = this.getUser();
    if (!user || !user.id) {
      throw new Error("No authenticated user found");
    }
    return user.id;
  },

  setAuth(token, user) {
    if (!token || !user) {
      throw new Error("setAuth requires both token and user");
    }
    localStorage.setItem(JWT_KEY, token);
    localStorage.setItem(USER_STORAGE_KEY, JSON.stringify(user));
    queryClient.setQueryData(USER_QUERY_KEY, user);
  },

  clearToken() {
    localStorage.removeItem(JWT_KEY);
    localStorage.removeItem(USER_STORAGE_KEY);
    queryClient.removeQueries(USER_QUERY_KEY);
  },

  clearAll() {
    this.clearToken();
  },
};
