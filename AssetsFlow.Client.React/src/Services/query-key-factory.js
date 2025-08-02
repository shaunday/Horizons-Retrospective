import { authStorage } from "@services/authStorage"; 

const TRADE_KEY = "Trade";
const TRADES_ARRAY_KEY = "AllTrades";
const TRADE_IDS_ARRAY_KEY = "AllTradesByIds";

function safeGetUserId() {
  try {
    return authStorage.getUserId();
  } catch {
    return "";
  }
}

export const tradeKeysFactory = {
  getKeyForAllTrades: () => {
    const userId = safeGetUserId();
    return [userId, TRADES_ARRAY_KEY];
  },
  getKeyForAllTradesByIds: () => {
    const userId = safeGetUserId();
    return [userId, TRADE_IDS_ARRAY_KEY];
  },
  getTradeAndIdArrayKey: (tradeId) => {
    const userId = safeGetUserId();
    return [userId, TRADE_KEY, tradeId];
  },
};