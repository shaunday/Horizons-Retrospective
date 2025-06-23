import React from "react";
import { Paper, Text } from "@mantine/core";
import clsx from "clsx";
import * as Constants from "@constants/journalConstants";
import { ProcessingStatus } from "@constants/Constants";
import ProcessingAndSuccessMessage from "@components/Processing/ProcessingAndSuccessMessage";
import ValueWrapper from "./ValueWrapper";
import { useContentUpdateMutation } from "@hooks/Journal/Entry/useContentUpdateMutation";
import { dataElementContentParser } from "@services/dataElementContentParser";

function DataElement({ cellInfo, overviewType }) {
  const isOverview = overviewType != Constants.OverviewType.NONE;
  const { contentValue } = dataElementContentParser(cellInfo);

  const { contentUpdateMutation, processingStatus } =
    useContentUpdateMutation(cellInfo);

  const initiateMutation = (newValue) => {
    contentUpdateMutation.mutate(newValue);
  };

  return (
    <Paper
      shadow="xs"
      radius="md"
      withBorder
      className={clsx(
        "container-with-centered-content px-1 min-w-20 flex gap-1 cursor-default",
        {
          "max-w-60": isOverview,
          "max-w-36": !isOverview,
          "flex-row": isOverview,
          "flex-col": !isOverview,
          "bg-red-50 border-red-200":
            !contentValue &&
            overviewType != Constants.OverviewType.TRADE_OVERVIEW,
          "pointer-events-none":
            processingStatus === ProcessingStatus.PROCESSING,
          "pointer-events-auto":
            processingStatus !== ProcessingStatus.PROCESSING,
        }
      )}
    >
      <Text className="whitespace-nowrap">
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
