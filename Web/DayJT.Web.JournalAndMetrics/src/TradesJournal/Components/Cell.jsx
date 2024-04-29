import React from 'react';
import * as TradesApiAccess from './../Services/TradesApiAccess'

function Cell({cellInfo}) {
    
  const handleKeyPress = e => {
    if (e.key === "Enter") {
      updateCellContent();
    }
  };

  const updateCellContent = e => {
    //todo update via store
  };

    return (
        <div id="cell">
            <div id="cellTitle">{cellInfo.ContentWrapper.Title}</div>
            <input id="cellInput" type="text" value={cellInfo.ContentWrapper.Content} onKeyDown={handleKeyPress}/>
        </div> )};

export default Cell;