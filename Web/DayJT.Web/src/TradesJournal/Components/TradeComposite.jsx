import {React, memo} from 'react';
import TradeElement from './TradeElement';

export default function TradeComposite({tradeComposite}) {
    
    return (
        <div id="tradeComposite">
            <ul>
                {tradeComposite.TradeElements.map(step=> (
                    <li key={step.id}>
                        <TradeElement tradeStep={step}/>
                    </li>
                    ))}
            </ul>
        </div> )};