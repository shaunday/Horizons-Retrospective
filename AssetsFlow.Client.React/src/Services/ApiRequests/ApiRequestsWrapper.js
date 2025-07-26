import { authStorage } from "@services/authStorage";

export const baseURL = import.meta.env.VITE_API_URL;
export const tradesBaseURL = `${baseURL}${import.meta.env.VITE_JOURNAL_TRADES_SUFFIX}`;

export async function request(url, options = {}, retry = true) {
  const token = authStorage.getToken();

  const headers = {
    "Content-Type": "application/json",
    ...(token ? { Authorization: `Bearer ${token}` } : {}),
    ...options.headers,
  };

  let response;

  try {
    response = await fetch(url, {
      ...options,
      headers,
    });
  } catch (networkError) {
    // This catches DNS errors, connection refused, etc.
    throw new Error(`Network error: ${networkError.message}`);
  }

  // Handle 401 and retry with refresh
  if (response.status === 401 && retry) {
    try {
      const refreshed = await refreshToken(); // Call refresh endpoint
      if (refreshed?.token && refreshed?.user) {
        authStorage.setAuth(refreshed.token, refreshed.user);
        return request(url, options, false);
      }
    } catch (refreshError) {
      authStorage.clearAll();
      // TODO: redirect to login if needed
      throw new Error("Session expired. Please log in again.");
    }
  }

  if (!response.ok) {
    // Try to read text for better error info
    const errorText = await response.text().catch(() => "");
    throw new Error(`Request failed: ${response.status} ${response.statusText}\n${errorText}`);
  }

  // If response body is empty (204 No Content), return null
  if (response.status === 204) return null;

  try {
    return await response.json();
  } catch (jsonError) {
    throw new Error("Failed to parse response as JSON.");
  }
}

export async function refreshToken() {
  const response = await request(`${baseURL}auth/refresh`, {
    method: "POST",
    credentials: "include", // Only if your refresh token is in a cookie
  },
    +    false
  );
  return response;
}
