export const baseURL = import.meta.env.VITE_API_URL;
export const tradesBaseURL = `${baseURL}${import.meta.env.VITE_JOURNAL_TRADES_SUFFIX}`;

export async function request(url, options = {}) {
  const response = await fetch(url, {
    headers: { "Content-Type": "application/json" },
    ...options,
  });

  if (!response.ok) {
    throw new Error(`Request failed: ${response.statusText}`);
  }

  return response.json();
}