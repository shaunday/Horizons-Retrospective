import { useMutation } from "@tanstack/react-query";
import { updateEntry } from "@services/ApiRequests/entryApiAccess";
import {ProcessingStatus} from "@constants/constants";
import { useProcessingWrapper } from "@hooks/useProcessingWrapper";

export function useContentUpdateMutation(cellInfo, onDataUpdateSuccess) {
  const { processingStatus, setNewStatus } = useProcessingWrapper(ProcessingStatus.NONE);

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
      if (onDataUpdateSuccess) {
        onDataUpdateSuccess(response);
      }
      setNewStatus(ProcessingStatus.SUCCESS); // Set to SUCCESS when mutation is successful
    },
  });

  return { contentUpdateMutation, processingStatus };
}