import { tradesBaseURL, request } from "./ApiRequestsWrapper";

export async function getAllTrades() {
  return request(`${tradesBaseURL}`, { method: "GET" });
}

export async function getTradeById(tradeId) {
  return request(`${tradesBaseURL}/${tradeId}`, { method: "POST" });
}

export async function addTradeComposite() {
  return request(`${tradesBaseURL}`, { method: "POST" });
}