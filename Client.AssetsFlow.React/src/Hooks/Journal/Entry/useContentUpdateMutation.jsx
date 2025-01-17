import { useMutation } from "@tanstack/react-query";
import { updateEntry } from "@services/tradesApiAccess";
import { useState } from "react";

export function useContentUpdateMutation(cellInfo, onCellUpdate) {
  const [processing, setProcessing] = useState(false);
  const [success, setSuccess] = useState(false);

  const contentUpdateMutation = useMutation({
    mutationFn: (newContent) => {
      setProcessing(true);
      setSuccess(false);
      updateEntry(cellInfo.Id, newContent, "");
    },
    onError: (error) => {
      setProcessing(false);
      console.error("Error updating content:", error);
    },
    onSuccess: (updatedEntry, newSummary) => {
      onCellUpdate(updatedEntry, newSummary);
      setProcessing(false);
      setSuccess(true); // Mark success
      setTimeout(() => setSuccess(false), 2000); // Clear success state after 2 seconds
    },
  });

  return { contentUpdateMutation, processing, success };
}
