import { useMutation } from "@tanstack/react-query";
import { updateEntry } from "@services/entryApiAccess";
import * as Constants from "@constants/journalConstants";
import { useState } from "react";

export function useContentUpdateMutation(cellInfo, onDataUpdateSuccess) {
  const [processing, setProcessing] = useState(false);
  const [success, setSuccess] = useState(false);

  const contentUpdateMutation = useMutation({
    mutationFn: (newContent) => {
      setProcessing(true);
      setSuccess(false);
      const updatedData = updateEntry(cellInfo.id, newContent, "");
      return updatedData;
    },
    onError: (error) => {
      setProcessing(false);
      console.error("Error updating content:", error);
    },
    onSuccess: (response) => {
      if (onDataUpdateSuccess) {
        onDataUpdateSuccess(response);
      }
      setProcessing(false);
      setSuccess(true); // Mark success
      setTimeout(() => setSuccess(false), 2000); // Clear success state after 2 seconds
    },
  });

  return { contentUpdateMutation, processing, success };
}
