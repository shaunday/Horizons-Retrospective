import React from 'react'
import FilterControl from '../Components/FilterControl'
import TradeComposite from '../Components/TradeComposite';

export default function JournalContainer({trades}) {
    
    const TradeCompositeMemo = memo(TradeComposite)

    return (
        <div id="journalMainBody">
        <FilterControl/>
        
        const trades ="hi";
        <ul>
            {trades.map(composite=> (
                    <li key={composite.id}>
                        <TradeCompositeMemo tradeComposite={composite}/>
                    </li>
                    ))}
        </ul>
        <button className="button-38" type="button" 
                    style={{ justifyContent: 'center'}}>Add a Trade</button>
        </div> )};

