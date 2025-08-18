import { useMutation } from "@tanstack/react-query";
import { TradeActions } from "@constants/journalConstants";
import {
  addReduceInterimPosition,
  closeTrade,
  addEvaluation,
} from "@services/ApiRequests/tradeApiAccess";
import { ProcessingStatus } from "@constants/constants";
import { useUpdateTradeStatusFromResponse } from "./useUpdateTradeStatusFromResponse";
import { useCacheNewElement } from "@hooks/Journal/Element/useCacheNewElement";
import * as Constants from "@constants/journalConstants";
import { useProcessingWrapper } from "@hooks/useProcessingWrapper";

export const useTradeActionMutation = (tradeComposite) => {
  const updateTradeStatuses = useUpdateTradeStatusFromResponse(tradeComposite.id);
  const onElementUpdate = useCacheNewElement(tradeComposite);
  const { processingStatus, setNewStatus } = useProcessingWrapper(ProcessingStatus.NONE);

  const tradeActionMutation = useMutation({
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
    onMutate: () => {
      setNewStatus(ProcessingStatus.PROCESSING); // Set to PROCESSING when mutation starts
    },
    onError: (error) => {
      console.error("Error adding/reducing position:", error);
    },
    onSuccess: ({ response }, { action }) => {
      updateTradeStatuses(response);
      const newElement = response[Constants.NEW_ELEMENT_RESPONSE_TAG];
      if (newElement) {
        onElementUpdate(newElement);
      }
      setNewStatus(ProcessingStatus.SUCCESS);
    },
  });

  return { tradeActionMutation, processingStatus };
};
