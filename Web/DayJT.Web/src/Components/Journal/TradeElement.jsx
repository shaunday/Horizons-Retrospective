import React, { useState, useCallback } from "react";
import Cell from "./Cell";
import { useTradeStateAndManagement } from "@hooks/Composite/useTradeStateAndManagement";
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
  const [isCollapsed, setIsCollapsed] = useState(false);

  const initialEntriesValue = tradeElement[Constants.TRADE_ENTRIES_STRING];

  const processCellUpdate = useCallback(
    (data) => {
      //todo handle inter-connectedness
      onElementContentUpdate(data);
    },
    [onElementContentUpdate]
  );

  const toggleCollapse = useCallback(() => {
    setIsCollapsed((prev) => !prev);
  }, []);

  return (
    <>
      <div onClick={toggleCollapse} style={{ cursor: "pointer" }}>
        {isCollapsed ? "▼" : "▲"}
      </div>
      <ul style={listStyle}>
        {initialEntriesValue
          .filter(
            (entry) =>
              !isCollapsed || entry[Constants.RELEVANT_FOR_ORVERVIEW_STRING]
          ) // Filter if collapsible is true, otherwise show all
          .map((entry) => (
            <li key={entry.id} style={listItemStyle}>
              <Cell cellInfo={entry} onCellUpdate={processCellUpdate} />
            </li>
          ))}
      </ul>
    </>
  );
}

export default React.memo(TradeElement);
