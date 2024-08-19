import React, { useState } from 'react';
import { useMutation } from '@tanstack/react-query'
import * as tradesApiAccess from '../..//tradesApiAccess'

export default function Cell({cellInfo, onCellUpdate}) {

  const [displayValue, setDisplayValue] = useState(cellInfo.ContentWrapper.Content );
  
  //todo - history 
  const [localCellInfo, setLocalCellInfo] = useState(cellInfo);

  const contentUpdateMutation = useMutation({
    scope: {
      id: 'entry' + cellInfo.Id,
    },
    mutationFn: (newContent) => {
      tradesApiAccess.UpdateComponent(cellInfo.Id, newContent, "")
      //mark process. somehow
    },
    onError: (error, variables, context) => {
      // An error happened!
      //log, revert
    },
    onSuccess: (updatedCellInfo, newSummary) => {
      setLocalCellInfo(updatedCellInfo);
      onCellUpdate(newSummary);
    },
  })

  const handleKeyPress = e => {
    if (e.key === "Enter") {
      initiateContentUpdate(e.target.value);
    }
  };

  const initiateContentUpdate = value => {
    contentUpdateMutation.mutate(value)
  };

    return (
        <div id="cell">
            <div id="cellTitle">{cellInfo.ContentWrapper.Title}</div>
            <input 
              id="cellInput" 
              type="text" 
              value={displayValue} 
              onChange={(e) => setDisplayValue(e.target.value)}
              onKeyDown={handleKeyPress}/>
        </div> )};




