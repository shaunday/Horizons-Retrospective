import { useMutation } from "@tanstack/react-query";
import { updateEntry } from "@services/ApiRequests/entryApiAccess";
import {ProcessingStatus} from "@constants/constants";
import { useProcessingWrapper } from "@hooks/useProcessingWrapper";
import { useCacheUpdatedEntry } from "@hooks/Journal/Entry/useCacheUpdatedEntry";
import { useUpdateTradeStatusFromResponse } from "@hooks/Journal/Composite/useUpdateTradeStatusFromResponse";
import { useUpdateElementCacheData } from "@hooks/Journal/Element/useUpdateElementCacheData";
import { newStatesResponseParser } from "@services/newStatesResponseParser";
import * as Constants from "@constants/journalConstants";

export function useContentUpdateMutation(cellInfo) {
  const { processingStatus, setNewStatus } = useProcessingWrapper(ProcessingStatus.NONE);
  const tradeId = cellInfo[Constants.DATA_COMPOSITEFK_STRING];
  const tradeElementId = cellInfo[Constants.DATA_TRADE_ELEMENTFK_STRING];
  const cacheUpdatedEntry = useCacheUpdatedEntry(tradeId);
  const updateElementProp = useUpdateElementCacheData(tradeId, tradeElementId);
  const updateTradeStatuses = useUpdateTradeStatusFromResponse(tradeId);

  const contentUpdateMutation = useMutation({
    mutationFn: (newContent) => {
      return updateEntry(cellInfo.id, newContent, "");
    },
    onMutate: () => {
      setNewStatus(ProcessingStatus.PROCESSING); // Set to PROCESSING when mutation starts
    },
    onError: (error) => {
      setNewStatus(ProcessingStatus.NONE); // Reset to NONE on error
      console.error("Error updating content:", error);
    },
    onSuccess: (response) => {
      const updatedEntry = response[Constants.NEW_DATA_RESPONSE_TAG];
      if (updatedEntry) {
        cacheUpdatedEntry(updatedEntry);
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

  return { contentUpdateMutation, processingStatus };
}