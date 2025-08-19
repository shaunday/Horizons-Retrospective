import { authStorage } from "@services/authStorage";

const TRADE_BY_ID_KEY = "TradeById";
const TRADES_ARRAY_KEY = "AllTrades";
const USER_DATA_KEY = "UserData"; // new key

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
  getKeyForTradeById: (tradeId) => {
    const userId = safeGetUserId();
    return [userId, TRADE_BY_ID_KEY, tradeId];
  },
  getKeyForForAllTradesByIds: () => {
    const userId = safeGetUserId();
    return [userId, TRADE_BY_ID_KEY];
  },
};

export const userDataKeysFactory = {
  getKeyForUserData: () => {
    const userId = safeGetUserId();
    return [userId, USER_DATA_KEY];
  },
};