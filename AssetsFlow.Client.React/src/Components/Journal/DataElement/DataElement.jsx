import React from "react";
import { Card, Text } from '@mantine/core';
import * as Constants from "@constants/journalConstants";
import { ProcessingStatus } from "@constants/Constants";
import ProcessingAndSuccessMessage from "@components/Processing/ProcessingAndSuccessMessage";
import ValueWrapper from "./ValueWrapper";
import { useContentUpdateMutation } from "@hooks/Journal/Entry/useContentUpdateMutation";

const textStyle = {
  whiteSpace: "nowrap",
  overflow: "hidden",
  textOverflow: "ellipsis",
};

function DataElement({ cellInfo, onCellUpdate }) {
  const isOverview = onCellUpdate === undefined;

  const { contentUpdateMutation, processingStatus } =
    useContentUpdateMutation(cellInfo, (cellUpdateResponse) => {
      if (onCellUpdate) {
        onCellUpdate(cellUpdateResponse);
      }
    });

  const initiateMutation = (newValue) => {
    contentUpdateMutation.mutate(newValue);
  };

  const containerStyle = {
    ...(processingStatus === ProcessingStatus.SUCCESS ? { borderColor: "green" } : {}),
    pointerEvents: processingStatus === ProcessingStatus.PROCESSING ? "none" : "auto",
    opacity: processingStatus === ProcessingStatus.PROCESSING ? 0.5 : 1,
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
      <ProcessingAndSuccessMessage status={processingStatus} />
    </Card>
  );
}
export default React.memo(DataElement);
