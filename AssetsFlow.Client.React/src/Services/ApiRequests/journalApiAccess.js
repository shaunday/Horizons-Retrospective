import { tradesBaseURL, request } from "./ApiRequestsWrapper";
import { authStorage } from "@services/authStorage";

export async function getAllTrades() {
  const userId = authStorage.getUserId();
  return request(`${tradesBaseURL}?userId=${userId}`, { method: "GET" });
}

export async function getFilteredTrades(filters) {
  const userId = authStorage.getUserId();
  return request(`${tradesBaseURL}/filtered?userId=${userId}`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(filters),
  });
}


export async function getTradeById(tradeId) {
  return request(`${tradesBaseURL}/${tradeId}`, { method: "GET" });
}

export async function addTradeComposite() {
  const userId = authStorage.getUserId();
  return request(`${tradesBaseURL}?userId=${userId}`, { method: "POST" });
}