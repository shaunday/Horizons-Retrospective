import React, { useCallback } from "react";
import Cell from "./Cell";
import * as Constants from "@constants/journalConstants";

const listStyle = {
  display: "flex",
  listStyleType: "none", // Removes default list bullets
  padding: 0, // Removes default padding
  margin: 0, // Removes default margin
};

const listItemStyle = {
  marginRight: "10px", // Adds spacing between items
  border: "1px solid lightblue", // Adds a solid black border
  padding: "5px", // Adds some padding inside the border
  borderRadius: "4px", // Optional: Adds rounded corners
};

function TradeElement({ tradeElement, onElementContentUpdate }) {
  const initialEntriesValue = tradeElement[Constants.TRADE_ENTRIES_STRING];

  const processCellUpdate = useCallback(
    (data) => {
      //todo handle inter-connectdness
      onElementContentUpdate(data);
    },
    [onElementContentUpdate]
  );

  return (
    <div id="tradeElement">
      <ul style={listStyle}>
        {initialEntriesValue.map((entry) => (
          <li key={entry.id} style={listItemStyle}>
            <Cell cellInfo={entry} onCellUpdate={processCellUpdate} />
          </li>
        ))}
      </ul>
    </div>
  );
}

export default React.memo(TradeElement);
