import React from 'react';
import Cell from './Cell';

export default function TradeComponent({tradeComponent}) {

    const CellMemo = memo(Cell)

    return (
        <div id="tradeComponent">
            {/* todo add check vs expanded tradeid once i get store going */}
            <ul>
                {tradeComponent.TradeActionInfoCells.filter(actionInfo => actionInfo.IsRelevantForOverview || true).map(filteredActionInfo=> (
                    <li key={filteredActionInfo.id}>
                        <CellMemo cellInfo={filteredActionInfo}/>
                    </li>
                    ))}
             </ul>
        </div>)};