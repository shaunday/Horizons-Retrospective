import axios from "axios";

const baseURL = import.meta.env.VITE_API_URL;
const tradesSuffix = import.meta.env.VITE_JOURNAL_TRADES_SUFFIX;

const tradesClient = axios.create({ baseURL: `${baseURL}${tradesSuffix}` });


export async function addEvaluation(tradeId) {
  const response = await tradesClient.post(`${tradeId}/evaluate`);
  return response.data;
}

export async function addReduceInterimPosition(tradeId, isAdd) {
  const response = await tradesClient.post(`${tradeId}`, isAdd);
  return response.data;
}

export async function deleteInterimPosition(tradeId) {
  const response = await tradesClient.delete(`${tradeId}`);
  return response.data;
}

export async function closeTrade(tradeId, closingPrice) {
  const response = await tradesClient.post(`${tradeId}/close?closingPrice=${encodeURIComponent(closingPrice)}`);
  return response.data;
}