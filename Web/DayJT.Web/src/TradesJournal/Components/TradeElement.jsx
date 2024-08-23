import React from 'react';
import Cell from './Cell';
import * as Constants from '@constants/constants';

export default function TradeElement({tradeElement, onElementUpdate}) {
    const initialEntriesValue = tradeElement[Constants.TRADE_ENTRIES_STRING];

    const onCellUpdate = (data) => {
        //todo handle inter-connectdness
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
                {initialEntriesValue.map(entry=> (
                    <li key={entry.id} style={listItemStyle}>
                        <Cell cellInfo={entry} onCellUpdate={onCellUpdate}/>
                    </li>
                    ))}
             </ul>
        </div>)};