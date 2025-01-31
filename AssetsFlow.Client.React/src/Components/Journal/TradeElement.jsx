import React, { useState, useCallback } from "react";
import * as Constants from "@constants/journalConstants";
import DataElement from "./DataElement/DataElement";

const listStyle = {
  display: "flex",
  listStyleType: "none", // Removes default list bullets
  flexWrap: "wrap", // Allow items to wrap to the next line
  padding: 0, // Removes default padding
  margin: 0, // Removes default margin
};

const listItemStyle = {
  border: "1px solid grey", 
  padding: "5px", 
  borderRadius: "4px", 
  marginLeft: "5px",
   maxWidth: "95px", // Maximum width is 100px
  width: "100%", // Allow it to shrink if needed
  flexShrink: 1, // Allow the item to shrink if needed
};

function TradeElement({ tradeElement, onElementContentUpdate }) {
  const processCellUpdate = useCallback(
    (data) => {
      //todo handle inter-connectedness
      onElementContentUpdate(data);
    },
    [onElementContentUpdate]
  );

  return (
    <>
      <ul style={listStyle}>
      {tradeElement[Constants.TRADE_ENTRIES_STRING]
          .map((entry, index) => (
            <li key={entry.id} style={listItemStyle}>
              <DataElement cellInfo={entry} onCellUpdate={processCellUpdate} 
              style={index !== 0 ? { marginLeft: "10px" } : {}}/>
            </li>
          ))}
      </ul>
    </>
  );
}

export default React.memo(TradeElement);
