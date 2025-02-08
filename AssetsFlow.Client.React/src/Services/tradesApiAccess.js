import axios from "axios";

// Base URL and endpoints from environment variables
const baseURL = import.meta.env.VITE_API_URL;
const tradesSuffix = import.meta.env.VITE_JOURNAL_TRADES_SUFFIX;
const componentsSuffix = import.meta.env.VITE_JOURNAL_COMPONENTS_SUFFIX;

// Axios clients for trades and components
const tradesClient = axios.create({ baseURL: `${baseURL}${tradesSuffix}` });
const componentsClient = axios.create({ baseURL: `${baseURL}${componentsSuffix}` });

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
  const response = await tradesClient.get("");
  return response.data;
}

export async function addReduceInterimPosition(tradeId, isAdd) {
  const response = await tradesClient.post(`${tradeId}`, isAdd);
  return response.data;
}

export async function closeTrade(tradeId, closingPrice) {
  const response = await tradesClient.post(`${tradeId}/close`);
  return response.data;
}

export async function updateEntry(componentId, newContent, changeNote) {
  const payload = {
    content: newContent,
    info: changeNote,
  };
  const response = await componentsClient.put(`${componentId}`, newContent);
  return response.data;
}
