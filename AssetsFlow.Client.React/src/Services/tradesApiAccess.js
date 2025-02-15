import axios from "axios";

const baseURL = import.meta.env.VITE_API_URL;
const tradesSuffix = import.meta.env.VITE_JOURNAL_TRADES_SUFFIX;

const tradesClient = axios.create({ baseURL: `${baseURL}${tradesSuffix}` });

// API functions
export async function getAllTrades() {
  const response = await tradesClient.get("");
  return response.data;
}

export async function getTradeById(tradeId) {
  const response = await tradesClient.post(`${tradeId}`);
  return response.data;
}

export async function addTradeComposite() {
  const response = await tradesClient.post("");
  return response.data;
}

export async function addReduceInterimPosition(tradeId, isAdd) {
  const response = await tradesClient.post(`${tradeId}`, isAdd);
  return response.data;
}

export async function closeTrade(tradeId, closingPrice) {
  const response = await tradesClient.post(`${tradeId}/close?closingPrice=${encodeURIComponent(closingPrice)}`);
  return response.data;
}