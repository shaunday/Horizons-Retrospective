import { useMutation } from "@tanstack/react-query";
import { TradeActions } from "@constants/journalConstants";
import {
  addReduceInterimPosition,
  closeTrade,
  addEvaluation,
} from "@services/ApiRequests/tradeApiAccess"; 

export const useTradeActionMutation = (tradeComposite, onTradeActionSuccess) => {
  return useMutation({
    mutationFn: async ({ action, additionalParam }) => {
      let response;
      switch (action) {
        case TradeActions.ADD:
          response = await addReduceInterimPosition(tradeComposite.id, "true");
          break;
        case TradeActions.REDUCE:
          response = await addReduceInterimPosition(tradeComposite.id, "false");
          break;
        case TradeActions.EVALUATE:
          response = await addEvaluation(tradeComposite.id);
          break;
        case TradeActions.CLOSE:
          response = await closeTrade(tradeComposite.id, additionalParam);
          break;
        default:
          throw new Error("Unknown action");
      }
      return { action, response };
    },
    onError: (error) => {
      console.error("Error adding/reducing position:", error);
    },
    onSuccess: ({ response }, { action }) => {
      onTradeActionSuccess(response);
    },
  });
};