import React, { useState, useCallback } from "react";
import * as Constants from "@constants/journalConstants";
import DataElement from "./DataElement/DataElement";

const listStyle = {
  display: "flex",
  listStyleType: "none", // Removes default list bullets
  padding: 0, // Removes default padding
  margin: 0, // Removes default margin
};

const listItemStyle = {
  border: "1px solid lightblue", // Adds a solid black border
  padding: "5px", // Adds some padding inside the border
  borderRadius: "4px", // Optional: Adds rounded corners
};

function TradeElement({ tradeElement, onElementContentUpdate }) {
  const processCellUpdate = useCallback(
    (data) => {
      //todo handle inter-connectedness
      onElementContentUpdate(data);
    },
    [onElementContentUpdate]
  );

  const entries = tradeElement[Constants.TRADE_ENTRIES_STRING];

  if (!Array.isArray(entries)) {
    console.warn(`Expected an array for entries, but got:`, entries);
    return <div>No valid entries to display</div>;
  }

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
