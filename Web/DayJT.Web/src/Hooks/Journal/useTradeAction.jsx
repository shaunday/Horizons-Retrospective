import { useMutation } from "@tanstack/react-query";
import {
  addPosition,
  reducePosition,
  closeTrade,
} from "@services/tradesApiAccess";
import * as Constants from "@constants/journalConstants";

const useTradeAction = (tradeComposite, onElementAddition) => {
  return useMutation({
    mutationFn: async (action) => {
      switch (action) {
        case Constants.TradeActions.ADD:
          await addPosition(tradeComposite.Id);
          break;
        case Constants.TradeActions.REDUCE:
          await reducePosition(tradeComposite.Id);
          break;
        case Constants.TradeActions.CLOSE:
          await closeTrade(tradeComposite.Id);
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
