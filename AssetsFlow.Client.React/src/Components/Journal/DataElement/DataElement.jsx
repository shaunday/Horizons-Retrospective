import React from "react";
import { Paper, Text } from "@mantine/core";
import * as Constants from "@constants/journalConstants";
import { ProcessingStatus } from "@constants/Constants";
import ProcessingAndSuccessMessage from "@components/Processing/ProcessingAndSuccessMessage";
import ValueWrapper from "./ValueWrapper";
import { useContentUpdateMutation } from "@hooks/Journal/Entry/useContentUpdateMutation";

const getContainerStyles = (isOverview, processingStatus) => ({
  pointerEvents: processingStatus === ProcessingStatus.PROCESSING ? "none" : "auto",
  display: "flex",
  flexDirection: isOverview ? "row" : "column",
  maxWidth: isOverview ? 350 : 150,
});

function DataElement({ cellInfo, onCellUpdate }) {
  const isOverview = onCellUpdate === undefined;

  const { contentUpdateMutation, processingStatus } = useContentUpdateMutation(
    cellInfo,
    (cellUpdateResponse) => {
      if (onCellUpdate) {
        onCellUpdate(cellUpdateResponse);
      }
    }
  );

  const initiateMutation = (newValue) => {
    contentUpdateMutation.mutate(newValue);
  };

  const titleText = isOverview ? `${cellInfo[Constants.DATA_TITLE_STRING]}:` : cellInfo[Constants.DATA_TITLE_STRING];

  return (
    <Paper shadow="sm" p={3} radius="md" withBorder
      className="container-with-centered-content"
      style={getContainerStyles(isOverview, processingStatus)}>
      <Text className="no-overflow-text-style">
        {titleText}
      </Text>
      <ValueWrapper
        cellInfo={cellInfo}
        onValueChangeInitiated={!isOverview ? initiateMutation : undefined}
      />
      <ProcessingAndSuccessMessage status={processingStatus} />
    </Paper>
  );
}

export default React.memo(DataElement);
