import React from 'react'
import FilterControl from '../Components/FilterControl'
import TradeComponents from '../Components/TradeComposite';

export default function JournalContainer({trades}) {
    
    return (
        <div id="journalMainBody">
        <FilterControl/>
        
        const trades ="hi";
        <ul>
            {trades.map(composite=> (
                    <li key={composite.id}>
                        <TradeComposite tradeComposite={composite}/>
                    </li>
                    ))}
        </ul>
        <button className="button-38" type="button" 
                    style={{ justifyContent: 'center'}}>Add a Trade</button>
        </div> )};

