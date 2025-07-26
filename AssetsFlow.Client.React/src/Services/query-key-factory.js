import { authStorage } from "@services/authStorage"; 

const TRADE_KEY = "Trade";
const TRADES_ARRAY_KEY = "Trades";
const TRADE_IDS_ARRAY_KEY = "TradeIds";

function safeGetUserId() {
  try {
    return authStorage.getUserId();
  } catch {
    return "";
  }
}

export const tradeKeysFactory = {
  getTradesKey: () => {
    const userId = safeGetUserId();
    return [userId, TRADES_ARRAY_KEY];
  },
  getTradeIdsKey: () => {
    const userId = safeGetUserId();
    return [userId, TRADE_IDS_ARRAY_KEY];
  },
  getTradeAndIdArrayKey: (tradeId) => {
    const userId = safeGetUserId();
    return [userId, TRADE_KEY, tradeId];
  },
};