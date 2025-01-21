const TRADE_KEY = 'Trade';
const TRADES_ARRAY_KEY = 'Trades';
const TRADE_IDS_ARRAY_KEY = "TradeIds"

export const tradeKeysFactory = {
    tradesKey: [TRADES_ARRAY_KEY],
    tradeIdsKey: [TRADE_IDS_ARRAY_KEY],
    tradeByIdKey: (id) => [TRADE_KEY, tradeId]
}