import React, { useMemo, useCallback } from "react";
import { Group } from "@mantine/core"; 
import * as Constants from "@constants/journalConstants";
import ElementControls from "./ElementControls";
import GroupedEntriesList from "../DataElementGroups/GroupedEntriesList"; 

function TradeElement({ tradeElement, onElementContentUpdate, onElementAction }) {
  const isOverview = tradeElement.isOverview !== undefined;

  const isAllowControls = useMemo(() => {
    if (isOverview) return false;
    const elementType = tradeElement[Constants.ELEMENT_TYPE_STING];
    return elementType !== Constants.ElementType.ORIGIN && elementType !== Constants.ElementType.SUMMARY;
  }, [tradeElement]);

  const processTimeStampUpdate = useCallback(
    (response) => {
      const updatedTimestamp = response[Constants.NEW_TIMESTAMP_RESPONSE_TAG];
      if (updatedTimestamp) {
        tradeElement[Constants.ELEMENT_TIMESTAMP_STING] = updatedTimestamp;
      }
    },
    [onElementContentUpdate]
  );

  const processCellUpdate = useCallback(
    (cellUpdateResponse) => {
      if (!isOverview) onElementContentUpdate(cellUpdateResponse);
      processTimeStampUpdate(cellUpdateResponse);
    },
    [onElementContentUpdate]
  );

  const processElementActionSuccess = useCallback((ElementActionResponse) => {
    processTimeStampUpdate(ElementActionResponse);
    onElementAction(ElementActionResponse);
  });

  return (
    <Group wrap="wrap" spacing={10}>
      <GroupedEntriesList
        entries={tradeElement[Constants.TRADE_ENTRIES_STRING]}
        isOverview={isOverview}
        processCellUpdate={processCellUpdate}
      />

      {isAllowControls &&
        <ElementControls tradeElement={tradeElement} onActionSuccess={processElementActionSuccess} />
      }
    </Group>
  );
}

export default React.memo(TradeElement);