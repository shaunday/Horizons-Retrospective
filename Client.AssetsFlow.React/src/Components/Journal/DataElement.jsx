import React, { useState } from "react";
import * as Constants from "@constants/journalConstants";
import { useContentUpdateMutation } from "@hooks/Entry/useContentUpdateMutation";
import SuccessMessage from "@components/SuccessMessage";

const MemoizedSuccessMessage = React.memo(SuccessMessage);

function DataElement({ cellInfo, onCellUpdate }) {
  const [displayValue, setDisplayValue] = useState(
    cellInfo.ContentWrapper?.[Constants.DATAELEMENT_CONTENT_STRING] ?? ""
  );
  const [isEditing, setIsEditing] = useState(false);
  const { contentUpdateMutation, processing, success } =
    useContentUpdateMutation(cellInfo, onCellUpdate);

  // Focus input element when switching to edit mode
  const focusInput = () => {
    const input = document.getElementById("cellInput");
    if (input) input.focus();
  };

  // Handle click on "Edit" or "Apply" button
  const handleEditClick = (e) => {
    e.stopPropagation(); // Prevent click from propagating to parent container
    if (isEditing) {
      initiateMutation();
    } else {
      focusInput(); // Focus input when entering edit mode
    }
    setIsEditing(!isEditing); // Toggle edit state
  };

  // Handle click inside the input field
  const handleInputClick = (e) => {
    if (isEditing) {
      e.stopPropagation(); // Prevent click from propagating to parent container
      focusInput(); // Focus input if in edit mode
    }
  };

  // Handle key press event (Enter to apply changes)
  const handleKeyPress = (e) => {
    if (e.key === "Enter") {
      initiateMutation();
    }
  };

  // Initiate mutation (apply changes)
  const initiateMutation = () => {
    contentUpdateMutation.mutate(displayValue);
  };

  return (
    <>
      <p>{cellInfo[Constants.DATAELEMENT_TITLE_STRING]}</p>
      <input
        id="cellInput"
        type="text"
        value={displayValue}
        onClick={handleInputClick}
        onChange={(e) => setDisplayValue(e.target.value)}
        onKeyDown={handleKeyPress}
        disabled={processing}
        style={{
          ...(success ? { borderColor: "green" } : {}),
          cursor: isEditing ? "text" : "pointer", // Show text cursor when editing
        }}
      />
      {!processing && (
        <button onClick={handleEditClick} style={{ cursor: "pointer" }}>
          {isEditing ? "✔️" : "✏️"}
        </button>
      )}
      {processing && <div className="spinner">Processing...</div>}
      {success && <MemoizedSuccessMessage />}
    </>
  );
}

export default React.memo(DataElement);
