import React from 'react';
import Cell from './Cell';

export default function TradeComponent({tradeComponent}) {

    const CellMemo = memo(Cell)

    return (
        <div id="tradeComponent">
            {/* todo add check vs expanded tradeid once i get store going */}
            {tradeComponent.TradeActionInfoCells.filter(actionInfo => actionInfo.IsRelevantForOverview || true).map(filteredActionInfo=> (
                <li>
                    <CellMemo cellInfo={filteredActionInfo}/>
                 </li>
                 ))}
        </div>)};