import { tradesBaseURL, request } from "./ApiRequestsWrapper";

export async function addEvaluation(tradeId) {
  return request(`${tradesBaseURL}/${tradeId}/evaluate`, { method: "POST" });
}

export async function addReduceInterimPosition(tradeId, isAdd) {
  return request(`${tradesBaseURL}/${tradeId}?isAdd=${encodeURIComponent(isAdd)}`, {
    method: "POST",
  });
}

export async function deleteInterimPosition(tradeId) {
  return request(`${tradesBaseURL}/${tradeId}`, { method: "DELETE" });
}

export async function closeTrade(tradeId, closingPrice) {
  return request(`${tradesBaseURL}/${tradeId}/close?closingPrice=${encodeURIComponent(closingPrice)}`, { method: "POST" });
}