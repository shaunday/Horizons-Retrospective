import React from "react";
import { Card, Text } from '@mantine/core';
import * as Constants from "@constants/journalConstants";
import { ProcessingStatus } from "@constants/Constants";
import ProcessingAndSuccessMessage from "@components/Processing/ProcessingAndSuccessMessage";
import ValueWrapper from "./ValueWrapper";
import { useContentUpdateMutation } from "@hooks/Journal/Entry/useContentUpdateMutation";

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
    pointerEvents: processingStatus === ProcessingStatus.PROCESSING ? "none" : "auto",
  };

  return (
    <Card shadow="sm" padding="xs" radius="md" withBorder>
      <div style={containerStyle}>
        <Text
          className="no-overflow-text-style centered-text"
          mb={5}
        >
          {cellInfo[Constants.DATA_TITLE_STRING]}
        </Text>
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
