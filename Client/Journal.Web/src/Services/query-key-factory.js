import * as Constants from "@constants/journalConstants";

export const tradeKeysFactory = {
    tradesKey: [Constants.TRADES_ARRAY_KEY],
    tradeIdsKey: [Constants.TRADE_IDS_ARRAY_KEY],
    tradeByIdKey: (id) => [Constants.TRADE_KEY, tradeId]
}