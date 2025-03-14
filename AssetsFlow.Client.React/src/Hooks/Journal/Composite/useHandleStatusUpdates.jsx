import { useCallback } from "react";
import * as Constants from "@constants/journalConstants";
import { useUpdateTradeCacheData } from "./useUpdateTradeCacheData";  
import { newStatesResponseParser } from "@services/newStatesResponseParser";

export const useHandleStatusUpdates = (tradeComposite) => {
  const { setNewData } = useUpdateTradeCacheData(tradeComposite.id);  

  const handleStatusUpdate = useCallback((response) => {
    const {
      newSummary, 
      newTradeStatus, 
      tradeIsPending, 
      tradeOpenedAt, 
      tradeClosedAt 
    } = newStatesResponseParser(response);

    setNewData(Constants.SUMMARY, newSummary);
    setNewData(Constants.TRADE_STATUS, newTradeStatus);
    setNewData(Constants.TRADE_ISPENDING, tradeIsPending);
    setNewData(Constants.COMPOSITE_OPENED_AT, tradeOpenedAt);
    setNewData(Constants.COMPOSITE_CLOSED_AT, tradeClosedAt);
  }, [setNewData]);

  return handleStatusUpdate;
};
