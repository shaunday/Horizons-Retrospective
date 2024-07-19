import {React, memo} from 'react';
import TradeComponent from './TradeComponent';

export default function TradeComposite({tradeComposite}) {
    
   const TradeCompositeMemo = memo(TradeComponent)

    return (
        <div id="tradeComposite">
            {tradeComposite.TradeComponents.map(component=> (
                <li>
                    <TradeCompositeMemo tradeComponent={component} key={component.id}/>
                 </li>
                 ))}
        </div> )};