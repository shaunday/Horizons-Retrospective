import React, { useState } from "react";
import { useMutation } from "@tanstack/react-query";
import { activateElementAPI } from "@services/elementApiAccess";
import { useProcessingWrapper } from "@hooks/useProcessingWrapper";
import {ProcessingStatus} from "@constants/constants";


export function useElementActivationMutation(onActivationSuccess) {
  const { processingStatus, setNewStatus } = useProcessingWrapper(ProcessingStatus.NONE);

  const elementActivationMutation = useMutation({
    mutationFn: (tradeElement) => {
      const activateResult = activateElementAPI(tradeElement.id);
      return activateResult;
    },
    onMutate: () => {
      setNewStatus(ProcessingStatus.PROCESSING); // Set to PROCESSING when mutation starts
    },
    onError: (error) => {
      setNewStatus(ProcessingStatus.NONE); // Reset to NONE on error
      console.error("Error activating:", error);
    },
    onSuccess: (response) => {
      if (response === true && onActivationSuccess) {
        onActivationSuccess();
      }
      setNewStatus(ProcessingStatus.SUCCESS)
    },
  });

  return { elementActivationMutation, processingStatus };
}
