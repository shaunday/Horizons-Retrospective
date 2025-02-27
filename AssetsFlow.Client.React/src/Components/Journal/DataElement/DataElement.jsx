import React, { useState } from "react";
import * as Constants from "@constants/journalConstants";
import { useContentUpdateMutation } from "@hooks/Entry/useContentUpdateMutation";
import SuccessMessage from "@components/SuccessMessage";
import ValueWrapper from "./ValueWrapper";

const MemoizedSuccessMessage = React.memo(SuccessMessage);

const baseContainerStyle = {
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

function DataElement({ cellInfo, onCellUpdate }) {
  const isOverview = onCellUpdate === undefined;

  const { contentUpdateMutation, processing, success } =
    useContentUpdateMutation(cellInfo, (cellUpdateResponse) => {
      if (onCellUpdate) {
        onCellUpdate(cellUpdateResponse);
      }
    });

  const initiateMutation = (newValue) => {
    contentUpdateMutation.mutate(newValue);
  };

  const containerStyle = {
    ...baseContainerStyle,
    ...(success ? { borderColor: "green" } : {}),
    pointerEvents: processing ? "none" : "auto",
    opacity: processing ? 0.5 : 1,
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
