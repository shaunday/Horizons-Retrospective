import React, { useState } from 'react';
import * as TradesApiAccess from './../Services/TradesApiAccess'

function Cell({cellInfo, onCellUpdateConfirmation}) {

  //1st/initial values
  let approvedValue = cellInfo.ContentWrapper.Content  
  let displayValue = cellInfo.ContentWrapper.Content  


  const handleKeyPress = e => {
    if (e.key === "Enter") {
      updateCellContent();
    }
  };

  const updateCellContent = e => {
    
  };

    return (
        <div id="cell">
            <div id="cellTitle">{cellInfo.ContentWrapper.Title}</div>
            <input id="cellInput" type="text" value={displayValue} onKeyDown={handleKeyPress}/>
        </div> )};

export default Cell;