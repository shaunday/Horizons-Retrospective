import { tradesBaseURL, request } from "./ApiRequestsWrapper";
import { DEMO_USER_ID } from "../demoUserService";

function getUserId() {
  return DEMO_USER_ID;
}

export async function getAllTrades() {
  const userId = getUserId();
  return request(`${tradesBaseURL}?userId=${userId}`, { method: "GET" });
}

export async function getTradeById(tradeId) {
  return request(`${tradesBaseURL}/${tradeId}`, { method: "GET" });
}

export async function addTradeComposite() {
  const userId = getUserId();
  return request(`${tradesBaseURL}?userId=${userId}`, { method: "POST" });
}