import React from 'react';
import Cell from './Cell';

function TradeComponent({tradeComponent}) {

    return (
        <div id="tradeComponent">
            {/* todo add check vs expanded tradeid once i get store going */}
            {tradeComponent.TradeActionInfoCells.filter(actionInfo => actionInfo.IsRelevantForOverview || true).map(filteredActionInfo=> (
                <li>
                    <Cell cellInfo={filteredActionInfo}/>
                 </li>
                 ))}
        </div>)};

export default TradeComponent;