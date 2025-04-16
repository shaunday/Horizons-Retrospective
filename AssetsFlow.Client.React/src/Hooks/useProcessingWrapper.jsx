import { useState, useRef } from "react";
import {ProcessingStatus} from "@constants/constants";

export function useProcessingWrapper(status) {
  const [processingStatus, setProcessing] = useState(status);
  const timeoutRef = useRef(null);

  const setNewStatus = (newStatus) => {
    if (newStatus === ProcessingStatus.SUCCESS) {
      // Clear previous timeout if any
      if (timeoutRef.current) {
        clearTimeout(timeoutRef.current);
      }
      // Set SUCCESS status and reset after 2 seconds
      setProcessing(newStatus);
      timeoutRef.current = setTimeout(() => setProcessing(ProcessingStatus.NONE), 1500);
    } else {
      setProcessing(newStatus);
    }
  };

  return { processingStatus, setNewStatus };
};