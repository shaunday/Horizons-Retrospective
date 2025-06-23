import { useCallback } from "react";
import * as Constants from "@constants/journalConstants";
import { useUpdateTradeCacheData } from "./useUpdateTradeCacheData";
import { newStatesResponseParser } from "@services/newStatesResponseParser";

export const useHandleStatusUpdates = (tradeComposite) => {
  const { setNewData } = useUpdateTradeCacheData(tradeComposite.id);

  const processTradeStatusUpdates = useCallback((response) => {
    const { newTradeStatus, tradeIsPending } = newStatesResponseParser(response);

    setNewData(Constants.TRADE_STATUS, newTradeStatus);
    setNewData(Constants.HasMissingContent, tradeIsPending);
  }, [setNewData]);

  return processTradeStatusUpdates;
};
