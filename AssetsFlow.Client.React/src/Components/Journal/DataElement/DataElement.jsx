import React from "react";
import * as Constants from "@constants/journalConstants";
import { useContentUpdateMutation } from "@hooks/Entry/useContentUpdateMutation";
import SuccessMessage from "@components/SuccessMessage";
import ValueWrapper from "./ValueWrapper";

const MemoizedSuccessMessage = React.memo(SuccessMessage);

function DataElement({ cellInfo, onCellUpdate }) {
  const { contentUpdateMutation, processing, success } =
    useContentUpdateMutation(cellInfo, onCellUpdate);

  // Initiate mutation (apply changes)
  const initiateMutation = (newValue) => {
    contentUpdateMutation.mutate(newValue);
  };

  // Conditionally apply styles
  const containerStyle = {
    ...(success ? { borderColor: "green" } : {}),
    pointerEvents: processing ? "none" : "auto", // Disable pointer events when processing
    opacity: processing ? 0.5 : 1, // Reduce opacity when processing
    display: "flex",  // Enables flexbox
    flexGrow: 1,
    justifyContent: "center", // Centers horizontally
    alignItems: "center", // Centers vertically
    width: "100%", // Full width minus 3px margin on each side
    margin: "0 3px", // 3px margin on left & right
  };

  return (
    <>
      <p
        style={{
          whiteSpace: "normal", // Allow text wrapping
          overflowWrap: "break-word", // Break long words onto the next line
        }}
      >
        {cellInfo[Constants.DATA_TITLE_STRING]}
      </p>
      <div style={containerStyle}>
        <ValueWrapper
          cellInfo={cellInfo}
          onValueChangeInitiated={initiateMutation}
        />
      </div>

      {processing && <div className="spinner">Processing...</div>}
      {success && <MemoizedSuccessMessage />}
    </>
  );
}

export default React.memo(DataElement);
