import React, { useMemo, useCallback } from "react";
import { Group } from "@mantine/core";
import * as Constants from "@constants/journalConstants";
import ElementControls from "./ElementControls";
import GroupedEntriesList from "../DataElementGroups/GroupedEntriesList"; 
import EntriesList from "../DataElementGroups/EntriesList"; 
import { newStatesResponseParser } from "@services/newStatesResponseParser"
import { useUpdateElementCacheData } from "@hooks/Journal/Element/useUpdateElementCacheData";

function TradeElement({ tradeElement, onElementContentUpdate, onElementAction }) {
  const isOverview = tradeElement.isOverview !== undefined;
  const { setNewData } = useUpdateElementCacheData(tradeElement[Constants.ELEMENT_COMPOSITEFK_STING], tradeElement.id);

  const isAllowControls = useMemo(() => {
    if (isOverview) return false;
    const elementType = tradeElement[Constants.ELEMENT_TYPE_STING];
    return elementType !== Constants.ElementType.ORIGIN && elementType !== Constants.ElementType.SUMMARY;
  }, [tradeElement]);

  const processTimeStampUpdate = useCallback(
    (response) => {
      const { elementsNewTimeStamp } = newStatesResponseParser(response);
      if (elementsNewTimeStamp) {
        setNewData(Constants.ELEMENT_TIMESTAMP_STING, elementsNewTimeStamp);
      }
    },
    [onElementContentUpdate]
  );

  const processCellUpdate = useCallback(
    (cellUpdateResponse) => {
      onElementContentUpdate(cellUpdateResponse); //kick response data up for composite handling
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
      {isOverview ? (
        <EntriesList
          entries={tradeElement[Constants.TRADE_ENTRIES_STRING]}
        />
      ) : (
        <GroupedEntriesList
          entries={tradeElement[Constants.TRADE_ENTRIES_STRING]}
          isOverview={isOverview}
          processCellUpdate={processCellUpdate}
        />
      )}

      {isAllowControls && 
        <ElementControls tradeElement={tradeElement} onActionSuccess={processElementActionSuccess} />
      }
    </Group>
  );
}

export default React.memo(TradeElement);