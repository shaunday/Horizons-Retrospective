import React, { useState } from "react";
import { useMutation } from "@tanstack/react-query";
import { activateElementAPI } from "@services/elementApiAccess";

export function useElementActivationMutation(onActivationSuccess) {
  const [processing, setProcessing] = useState(false);
  const [success, setSuccess] = useState(false);

  const elementActivationMutation = useMutation({
    mutationFn: (tradeElement) => {
      setProcessing(true);
      setSuccess(false);
      const activateResult = activateElementAPI(tradeElement.id);
      return activateResult;
    },
    onError: (error) => {
      setProcessing(false);
      console.error("Error activating:", error);
    },
    onSuccess: (response) => {
      if (response === true && onActivationSuccess) {
        onActivationSuccess();
      }
      setProcessing(false);
      setSuccess(true); // Mark success
      setTimeout(() => setSuccess(false), 2000); // Clear success state after 2 seconds
    },
  });

  return { elementActivationMutation, processing, success };
}
