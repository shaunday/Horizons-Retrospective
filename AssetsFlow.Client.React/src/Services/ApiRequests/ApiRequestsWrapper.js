import { authStorage } from "./authStorage"; // Import your wrapper

export const baseURL = import.meta.env.VITE_API_URL;
export const tradesBaseURL = `${baseURL}${import.meta.env.VITE_JOURNAL_TRADES_SUFFIX}`;

export async function request(url, options = {}, retry = true) {
  const token = authStorage.getToken();

  const headers = {
    "Content-Type": "application/json",
    ...(token ? { Authorization: `Bearer ${token}` } : {}),
    ...options.headers,
  };

  const response = await fetch(url, {
    ...options,
    headers,
  });

  // If token expired, try refreshing it
  if (response.status === 401 && retry) {
    try {
      const refreshed = await refreshToken(); // Call refresh endpoint
      if (refreshed?.token && refreshed?.user) {
        authStorage.setAuth(refreshed.token, refreshed.user);
        return request(url, options, false);
      }
    } catch (e) {
      // Refresh failed — log out the user
      authStorage.clearAuth(); // or logout()
      // todo redirect to login
    }
  }

  if (!response.ok) {
    throw new Error(`Request failed: ${response.statusText}`);
  }

  return response.json();
}

export async function refreshToken() {
  const response = await request(`${baseURL}auth/refresh`, {
    method: "POST",
    credentials: "include", // Only if your refresh token is in a cookie
  });
  return response;
}
