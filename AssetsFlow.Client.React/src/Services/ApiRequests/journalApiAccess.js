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
  const userId = getUserId();
  return request(`${tradesBaseURL}/${tradeId}?userId=${userId}`, { method: "POST" });
}

export async function addTradeComposite() {
  const userId = getUserId();
  return request(`${tradesBaseURL}?userId=${userId}`, { method: "POST" });
}