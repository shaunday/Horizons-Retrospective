import { DEMO_USER_ID } from "./demoUserService";

const TRADE_KEY = 'Trade';
const TRADES_ARRAY_KEY = 'Trades';
const TRADE_IDS_ARRAY_KEY = "TradeIds"

function getUserId() {
  return DEMO_USER_ID;
}

export const tradeKeysFactory = {
    getTradesKey: () => [getUserId(), TRADES_ARRAY_KEY],
    getTradeIdsKey: () => [getUserId(), TRADE_IDS_ARRAY_KEY],
    getTradeAndIdArrayKey: (tradeId) => [getUserId(), TRADE_KEY, tradeId]
}