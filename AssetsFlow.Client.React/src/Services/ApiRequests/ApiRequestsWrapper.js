export const baseURL = import.meta.env.VITE_API_URL;
export const tradesBaseURL = `${baseURL}${import.meta.env.VITE_JOURNAL_TRADES_SUFFIX}`;

export async function request(url, options = {}) {
  const token = localStorage.getItem('jwtToken');
  const headers = {
    "Content-Type": "application/json",
    ...(token ? { Authorization: `Bearer ${token}` } : {}),
    ...options.headers,
  };
  const response = await fetch(url, {
    ...options,
    headers,
  });

  if (!response.ok) {
    throw new Error(`Request failed: ${response.statusText}`);
  }

  return response.json();
}