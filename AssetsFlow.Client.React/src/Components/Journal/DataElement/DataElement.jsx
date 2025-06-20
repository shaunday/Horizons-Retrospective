import React from "react";
import { Paper, Text } from "@mantine/core";
import * as Constants from "@constants/journalConstants";
import { ProcessingStatus } from "@constants/Constants";
import ProcessingAndSuccessMessage from "@components/Processing/ProcessingAndSuccessMessage";
import ValueWrapper from "./ValueWrapper";
import { useContentUpdateMutation } from "@hooks/Journal/Entry/useContentUpdateMutation";
import { dataElementContentParser } from "@services/dataElementContentParser";

function DataElement({ cellInfo, onCellUpdate, overviewType }) {
  const isOverview = overviewType != Constants.OverviewType.NONE
  const { contentValue } = dataElementContentParser(cellInfo);

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

  const containerStyle = {
    pointerEvents:
      processingStatus === ProcessingStatus.PROCESSING ? "none" : "auto",
    display: "flex",
    gap: "5px",
    flexDirection: isOverview ? "row" : "column",
    border: !contentValue && overviewType != Constants.OverviewType.TRADE_OVERVIEW ? "1px solid red" : "none",
  };

  return (
    <Paper
      shadow="xs"
      radius="md"
      withBorder
      className={`container-with-centered-content px-1 ${isOverview ? 'max-w-60' : 'max-w-36'} min-w-18`}
      style={containerStyle}
    >
      <Text className="no-overflow-text-style">
        {cellInfo[Constants.DATA_TITLE_STRING]} {isOverview ? ":" : ""}
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
