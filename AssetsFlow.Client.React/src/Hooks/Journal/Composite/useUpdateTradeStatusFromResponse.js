import { useQueryClient } from "@tanstack/react-query";
import { tradeKeysFactory } from "@services/query-key-factory";
import { produce } from "immer";
import * as Constants from "@constants/journalConstants";
import { newStatesResponseParser } from "@services/newStatesResponseParser";

export function useUpdateTradeStatusFromResponse(tradeId) {
  const queryClient = useQueryClient();
  const updateTradeStatuses = (response) => {
    const { newTradeStatus, hasMissingContent, newSummary } = newStatesResponseParser(response);
    queryClient.setQueryData(
      tradeKeysFactory.getTradeAndIdArrayKey(tradeId),
      (oldTrade) =>
        produce(oldTrade, (draft) => {
          if (newTradeStatus !== undefined) draft[Constants.TRADE_STATUS] = newTradeStatus;
          if (hasMissingContent !== undefined) draft[Constants.HasMissingContent] = hasMissingContent;
          if (newSummary !== undefined) draft[Constants.TRADE_SUMMARY_STRING] = newSummary;
        })
    );
  };
  return updateTradeStatuses;
} 