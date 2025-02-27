import React from "react";
import * as Constants from "@constants/journalConstants";
import { useContentUpdateMutation } from "@hooks/Entry/useContentUpdateMutation";
import SuccessMessage from "@components/SuccessMessage";
import ValueWrapper from "./ValueWrapper";
import { Card, Text } from '@mantine/core';

const MemoizedSuccessMessage = React.memo(SuccessMessage);

const textStyle = {
  whiteSpace: "nowrap",
  overflow: "hidden",
  textOverflow: "ellipsis",
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
    ...(success ? { borderColor: "green" } : {}),
    pointerEvents: processing ? "none" : "auto",
    opacity: processing ? 0.5 : 1,
  };

  return (
    <Card shadow="sm" padding="lg" radius="md" withBorder>
      <Text style={textStyle}>{cellInfo[Constants.DATA_TITLE_STRING]}</Text>
      <div style={containerStyle}>
        <ValueWrapper
          cellInfo={cellInfo}
          onValueChangeInitiated={!isOverview ? initiateMutation : undefined}
        />
      </div>
      {processing && <div className="spinner">Processing...</div>}
      {success && <MemoizedSuccessMessage />}
    </Card>
  );
}
export default React.memo(DataElement);
