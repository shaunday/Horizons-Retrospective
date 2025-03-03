import React, { useMemo, useCallback } from "react";
import * as Constants from "@constants/journalConstants";
import DataElement from "../DataElement/DataElement";
import ElementControls from "./ElementControls";

const listStyle = {
  display: "flex",
  listStyleType: "none", // Removes default list bullets
  flexWrap: "wrap", // Allow items to wrap to the next line
  padding: 0, // Removes default padding
  margin: 0, // Removes default margin
};

const listItemStyle = {
  padding: "5px",
  width: "150px",
};

function TradeElement({ tradeElement, onElementContentUpdate, onElementAction }) {
  const isOverview = tradeElement.isOverview !== undefined;

  const isAllowControls = useMemo(() => {
    if (isOverview) return false;
    const elemntType = tradeElement[Constants.ELEMENT_TYPE_STING];
    return elemntType !== Constants.ElementType.ORIGIN && elemntType !== Constants.ElementType.SUMMARY;
  }, [tradeElement]);

  const processTimeStampUpdate = useCallback(
    (response) => {
      const updatedTimestamp = response[Constants.NEW_TIMESTAMP_RESPONSE_TAG];
      if (updatedTimestamp) {
        tradeElement[Constants.ELEMENT_TIMESTAMP_STING] = updatedTimestamp
      }
    },
    [onElementContentUpdate]
  );

  const processCellUpdate = useCallback(
    (cellUpdateResponse) => {
      // Handle inter-connectedness here - element might affect other elements

      if (!isOverview)
        onElementContentUpdate(cellUpdateResponse);

      processTimeStampUpdate(cellUpdateResponse);
    },
    [onElementContentUpdate]
  );

  const processElementActionSuccess  = useCallback((ElementActionResponse) =>
  {
    processTimeStampUpdate(ElementActionResponse);
    onElementAction(ElementActionResponse);
  });

  return (
    <>
      <ul style={listStyle}>
        {tradeElement[Constants.TRADE_ENTRIES_STRING]
          .map((entry, index) => (
            <li key={entry.id} style={listItemStyle}>
              <DataElement
                cellInfo={entry}
                {...(!isOverview && { onCellUpdate: processCellUpdate })}
                style={index !== 0 ? { marginLeft: "10px" } : {}}
              />
            </li>
          ))}
        {isAllowControls && 
        <li><ElementControls tradeElement={tradeElement} onActionSuccess={processElementActionSuccess} /></li>}
      </ul>
    </>
  );
}

export default React.memo(TradeElement);
