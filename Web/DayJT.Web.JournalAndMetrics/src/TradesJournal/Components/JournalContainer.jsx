import React from 'react'
import FilterControl from './FilterControl'
import * as TradesApiAccess from './../Services/TradesApiAccess'
import useAllTrades from './../useAllTrades'
import TradeComposite from './TradeComposite';

function JournalContainer() {
    
    return (
        <div id="journalMainBody">
        <FilterControl/>
        
        const trades ="hi";
        {trades.map(composite=> (
                <li>
                    <TradeComposite tradeComposite={composite}/>
                 </li>
                 ))}
        <button className="button-38" type="button" 
                    style={{ justifyContent: 'center'}}>Add a Trade</button>
        </div> )};


export default JournalContainer;