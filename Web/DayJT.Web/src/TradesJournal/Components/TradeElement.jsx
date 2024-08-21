import React from 'react';
import Cell from './Cell';
import { useQuery, useQueryClient } from '@tanstack/react-query'
import * as Constants from '../../Constants/constants'
import * as TradesApiAccess from './../Services/TradesApiAccess'

export default function TradeElement({tradeElement, onElementUpdate}) {

    const elementQueryKey = [Constants.TRADE_ELEMENT_KEY, tradeElement.TradeCompositeRefId, tradeElement.Id]
    const queryClient = useQueryClient()

    const { data: tradeEle } = useQuery({
        queryKey: elementQueryKey,
        initialData: tradeElement,
        refetchOnWindowFocus: false,
        queryFn: TradesApiAccess.getElement 
      })

      const onCellUpdate = (data) => {
        //handle inter-connectdness
        onElementUpdate(data);
      }

      const listStyle = {
        display: 'flex',
        listStyleType: 'none',  // Removes default list bullets
        padding: 0,             // Removes default padding
        margin: 0               // Removes default margin
      };
    
      const listItemStyle = {
        marginRight: '10px',    // Adds spacing between items
        border: '1px solid lightblue', // Adds a solid black border
        padding: '5px',        // Adds some padding inside the border
        borderRadius: '4px'    // Optional: Adds rounded corners
      };

    return (
        <div id="tradeElement">
            <ul style={listStyle}>
                {tradeEle.Entries.map(entry=> (
                    <li key={entry.id} style={listItemStyle}>
                        <Cell cellInfo={entry} onCellUpdate={onCellUpdate}/>
                    </li>
                    ))}
             </ul>
        </div>)};