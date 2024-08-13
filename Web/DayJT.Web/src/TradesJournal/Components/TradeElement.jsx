import React from 'react';
import Cell from './Cell';

export default function TradeElement({tradeElement}) {

    return (
        <div id="tradeElement">
            {/* todo add check vs expanded tradeid once i get store going */}
            <ul>
                {tradeElement.Entries.filter(entry => entry.IsRelevantForOverview || true).map(filteredEntry=> (
                    <li key={filteredEntry.id}>
                        <Cell cellInfo={filteredEntry}/>
                    </li>
                    ))}
             </ul>
        </div>)};