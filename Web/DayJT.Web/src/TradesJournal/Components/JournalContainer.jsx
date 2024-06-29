import React from 'react'
import FilterControl from './FilterControl'
import TradeComposite from './TradeComposite';

function JournalContainer({trades}) {
    
    const TradeCompositeMemo = memo(TradeComposite)

    return (
        <div id="journalMainBody">
        <FilterControl/>
        
        const trades ="hi";
        {trades.map(composite=> (
                <li>
                    <TradeCompositeMemo tradeComposite={composite} key={composite.id}/>
                 </li>
                 ))}
        <button className="button-38" type="button" 
                    style={{ justifyContent: 'center'}}>Add a Trade</button>
        </div> )};


export default JournalContainer;