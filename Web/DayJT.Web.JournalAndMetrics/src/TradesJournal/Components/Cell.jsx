import React from 'react';
import * as TradesApiAccess from './../Services/TradesApiAccess'

function Cell({cell}) {
    var content = cell.ContentWrapper.Content;
    
  const handleKeyPress = e => {
    if (e.key === "Enter") {
      updateCellContent();
    }
  };

  const updateCellContent = e => {
    TradesApiAccess.UpdateComponent(cell.Id, content, "no comment. ")
  };

    return (
        <div id="cell">
            <div id="cellTitle">{cell.Title}</div>
            <div id="cellContent" onKeyDown={handleKeyPress}>{content}</div>
        </div> )};

export default Cell;