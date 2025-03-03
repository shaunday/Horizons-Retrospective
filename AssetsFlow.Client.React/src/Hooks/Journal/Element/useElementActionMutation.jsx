import { useMutation } from "@tanstack/react-query";
import { ElementActions } from "@constants/journalConstants";
import { ProcessingStatus } from "@constants/constants";
import { activateElementAPI, } from "@services/elementApiAccess";
import { useProcessingWrapper } from "@hooks/useProcessingWrapper";

export function useElementActionMutation(tradeElement, onActionSuccess) {
  const { processingStatus, setNewStatus } = useProcessingWrapper(ProcessingStatus.NONE);

  const elementActionMutation = useMutation({
    mutationFn: ({ action }) => {
      switch (action) {
        case ElementActions.ACTIVATE:
          return activateElementAPI(tradeElement.id);
        case ElementActions.DELETE:
          return deleteElementAPI(tradeElement.id);
        default:
          throw new Error(`Unsupported action: ${action}`);
      }
    },
    onMutate: () => {
      setNewStatus(ProcessingStatus.PROCESSING); // Set to PROCESSING when mutation starts
    },
    onError: (error) => {
      setNewStatus(ProcessingStatus.NONE); // Reset to NONE on error
      console.error("Error performing action:", error);
    },
    onSuccess: (response) => {
      if (onActionSuccess) {
        onActionSuccess(response);
      }
      setNewStatus(ProcessingStatus.SUCCESS); // Set to SUCCESS when mutation is successful
    },
  });

  return { elementActionMutation, processingStatus };
}
