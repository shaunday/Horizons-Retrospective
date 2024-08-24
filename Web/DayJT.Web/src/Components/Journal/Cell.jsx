import { useState } from "react";
import { useMutation } from "@tanstack/react-query";
import { updateEntry } from "@services/tradesApiAccess";
import SuccessMessage from "@components/SuccessMessage";

export default function Cell({ cellInfo, onCellUpdate }) {
  const [displayValue, setDisplayValue] = useState(
    cellInfo.ContentWrapper.Content
  );
  const [processing, setProcessing] = useState(false);
  const [success, setSuccess] = useState(false);

  //todo - history

  const contentUpdateMutation = useMutation({
    scope: {
      id: "entry" + cellInfo.Id,
    },
    mutationFn: (newContent) => {
      setProcessing(true);
      setSuccess(false);
      updateEntry(cellInfo.Id, newContent, "");
    },
    onError: (error, variables, context) => {
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

  const handleKeyPress = (e) => {
    if (e.key === "Enter") {
      contentUpdateMutation.mutate(value);
    }
  };

  return (
    <div id="cell">
      <div id="cellTitle">{cellInfo.ContentWrapper.Title}</div>
      <input
        id="cellInput"
        type="text"
        value={displayValue}
        onChange={(e) => setDisplayValue(e.target.value)}
        onKeyDown={handleKeyPress}
        disabled={processing}
        style={success ? { borderColor: "green" } : {}}
      />
      {processing && <div className="spinner">Processing...</div>}
      {success && <SuccessMessage />}
    </div>
  );
}
