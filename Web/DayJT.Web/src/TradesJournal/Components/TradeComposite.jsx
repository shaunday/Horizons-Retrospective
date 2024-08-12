import {React, memo} from 'react';
import TradeComponent from './TradeComponent';

export default function TradeComposite({tradeComposite}) {
    
   const TradeCompositeMemo = memo(TradeComponent)

    return (
        <div id="tradeComposite">
            <ul>
                {tradeComposite.TradeComponents.map(component=> (
                    <li key={component.id}>
                        <TradeCompositeMemo tradeComponent={component}/>
                    </li>
                    ))}
            </ul>
        </div> )};