import { useMutation } from "@tanstack/react-query";
import { ElementActions } from "@constants/journalConstants";
import { ProcessingStatus } from "@constants/constants";
import { timestampElementAPI, deleteElementAPI } from "@services/ApiRequests/elementApiAccess";
import { useProcessingWrapper } from "@hooks/useProcessingWrapper";

export function useElementActionMutation(tradeElement, onActionSuccess) {
  const { processingStatus, setNewStatus } = useProcessingWrapper(ProcessingStatus.NONE);

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
      if (onActionSuccess) {
        onActionSuccess(action, response);
      }
      setNewStatus(ProcessingStatus.SUCCESS); // Set to SUCCESS when mutation is successful
    },
  });

  return { elementActionMutation, processingStatus };
}
