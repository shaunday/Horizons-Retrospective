import { useMutation } from "@tanstack/react-query";
import {
  addReduceInterimPosition,
  closeTrade,
} from "@services/tradesApiAccess";
import * as Constants from "@constants/journalConstants";

export const useTradeAction = (tradeComposite, onElementAddition) => {
  return useMutation({
    mutationFn: async (action) => {
      switch (action) {
        case Constants.TradeActions.ADD:
          return addReduceInterimPosition(tradeComposite.id, "true");
          break;
        case Constants.TradeActions.REDUCE:
          return addReduceInterimPosition(tradeComposite.id, "false");
          break;
        case Constants.TradeActions.CLOSE:
          return closeTrade(tradeComposite.id, "1001"); //todo find closing price
          break;
        default:
          throw new Error("Unknown action");
      }
    },
    onError: (error) => {
      console.error("Error adding/reducing position:", error);
    },
    onSuccess: (response) => {
      const { [Constants.NEW_ELEMENT_RESPONSE_TAG]: newElement, [Constants.NEW_SUMMARY_RESPONSE_TAG]: updatedSummary } = response;
      onElementAddition(newElement, updatedSummary);
    },
  });
};
