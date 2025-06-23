import { useMutation } from "@tanstack/react-query";
import { ElementActions } from "@constants/journalConstants";
import { ProcessingStatus } from "@constants/constants";
import { timestampElementAPI, deleteElementAPI } from "@services/ApiRequests/elementApiAccess";
import { useProcessingWrapper } from "@hooks/useProcessingWrapper";
import { useUpdateTradeStatusFromResponse } from "../Composite/useUpdateTradeStatusFromResponse";
import { useUpdateElementCacheData } from "@hooks/Journal/Element/useUpdateElementCacheData";
import { newStatesResponseParser } from "@services/newStatesResponseParser";
import { useRemoveElementFromTrade } from "@hooks/Journal/Element/useRemoveElementFromTrade"
import * as Constants from "@constants/journalConstants";

export function useElementActionMutation(tradeElement) {
  const { processingStatus, setNewStatus } = useProcessingWrapper(ProcessingStatus.NONE);
  const updateTradeStatuses = useUpdateTradeStatusFromResponse(tradeElement[Constants.ELEMENT_COMPOSITEFK_STING]);
  const updateElementProp = useUpdateElementCacheData(tradeElement[Constants.ELEMENT_COMPOSITEFK_STING], tradeElement.id);
    const removeElement = useRemoveElementFromTrade(tradeElement[Constants.ELEMENT_COMPOSITEFK_STING]);

  const elementActionMutation = useMutation({
    mutationFn: async ({ action }) => {
      let response;
      switch (action) {
        case ElementActions.TIMESTAMP:
          response = await timestampElementAPI(tradeElement.id); //todo pass new time
          break;
        case ElementActions.DELETE:
          response = await deleteElementAPI(tradeElement.id);
          break;
        default:
          throw new Error(`Unsupported action: ${action}`);
      }
      return { action, response };
    },
    onMutate: () => {
      setNewStatus(ProcessingStatus.PROCESSING); // Set to PROCESSING when mutation starts
    },
    onError: (error) => {
      setNewStatus(ProcessingStatus.NONE); // Reset to NONE on error
      console.error("Error performing action:", error);
    },
    onSuccess: ({ response }, { action }) => {
          if (action === ElementActions.DELETE) {
      removeElement(tradeElement.id);
    }
      // Update timestamp if present
      const { elementsNewTimeStamp } = newStatesResponseParser(response);
      if (elementsNewTimeStamp) {
        updateElementProp(Constants.ELEMENT_TIMESTAMP_STING, elementsNewTimeStamp);
      }
      updateTradeStatuses(response);
      setNewStatus(ProcessingStatus.SUCCESS); // Set to SUCCESS when mutation is successful
    },
  });

  return { elementActionMutation, processingStatus };
}
