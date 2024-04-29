import React from 'react';
import TradeComponent from './TradeComponent';

function TradeComposite({tradeComposite}) {
   
    return (
        <div id="tradeComposite">
            {tradeComposite.TradeComponents.map(component=> (
                <li>
                    <TradeComponent tradeComponent={component}/>
                 </li>
                 ))}
        </div> )};

export default TradeComposite;