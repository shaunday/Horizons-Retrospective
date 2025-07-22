import { authStorage } from "@services/authStorage"; 

const TRADE_KEY = "Trade";
const TRADES_ARRAY_KEY = "Trades";
const TRADE_IDS_ARRAY_KEY = "TradeIds";

export const tradeKeysFactory = {
  getTradesKey: () => [authStorage.getUserId(), TRADES_ARRAY_KEY],
  getTradeIdsKey: () => [authStorage.getUserId(), TRADE_IDS_ARRAY_KEY],
  getTradeAndIdArrayKey: (tradeId) => [authStorage.getUserId(), TRADE_KEY, tradeId],
};