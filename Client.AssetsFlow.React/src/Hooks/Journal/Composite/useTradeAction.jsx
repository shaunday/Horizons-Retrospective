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
          await addReduceInterimPosition(tradeComposite.Id, "true");
          break;
        case Constants.TradeActions.REDUCE:
          await addReduceInterimPosition(tradeComposite.Id, "false");
          break;
        case Constants.TradeActions.CLOSE:
          await closeTrade(tradeComposite.Id); //todo find closing price
          break;
        default:
          throw new Error("Unknown action");
      }
    },
    onError: (error) => {
      console.error("Error adding/reducing position:", error);
    },
    onSuccess: (newElement, newSummary) => {
      onElementAddition(newElement, newSummary);
    },
  });
};
