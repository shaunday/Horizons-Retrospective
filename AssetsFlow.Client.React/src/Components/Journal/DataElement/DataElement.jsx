import React, { useCallback } from "react";
import * as Constants from "@constants/journalConstants";
import { useContentUpdateMutation } from "@hooks/Entry/useContentUpdateMutation";
import SuccessMessage from "@components/SuccessMessage";
import ValueWrapper from "./ValueWrapper";

const MemoizedSuccessMessage = React.memo(SuccessMessage);

function DataElement({ cellInfo, onCellUpdate }) {
  const isOverview = onCellUpdate == undefined;
  const { contentUpdateMutation, processing, success } =
    useContentUpdateMutation(cellInfo, onCellUpdate);

  // Initiate mutation (apply changes)
  const initiateMutation = useCallback(
    (newValue) => {
      contentUpdateMutation.mutate(newValue);
    },
    [contentUpdateMutation] // Depend only on the mutation function
  );

  const containerStyle = {
    ...(success ? { borderColor: "green" } : {}),
    pointerEvents: processing ? "none" : "auto",
    opacity: processing ? 0.5 : 1,
    display: "flex",
    flexGrow: 1,
    justifyContent: "center",
    alignItems: "center",
    width: "100%",
    margin: "0 3px",
  };

  const textStyle = {
    whiteSpace: "nowrap",
    overflow: "hidden",
    textOverflow: "ellipsis",
    width: "100%",
  };

  return (
    <>
      <p style={textStyle}>{cellInfo[Constants.DATA_TITLE_STRING]}</p>
      <div style={containerStyle}>
      <ValueWrapper
          cellInfo={cellInfo}
          onValueChangeInitiated={!isOverview ? initiateMutation : undefined}
        />
      </div>

      {processing && <div className="spinner">Processing...</div>}
      {success && <MemoizedSuccessMessage />}
    </>
  );
}

export default React.memo(DataElement);
