import React from 'react'
import FilterControl from '../Components/FilterControl'
import TradeComposite from '../Components/TradeComposite';
import { useTrades } from './Hooks/useTrades'

export default function JournalContainer() {

    const { addTrade } = useTrades(); 

    return (
        <div id="journalMainBody">
        <FilterControl/>
        <ul>
            {trades.map(composite=> (
                    <li key={composite.id}>
                        <TradeComposite tradeComposite={composite}/>
                    </li>
                    ))}
        </ul>
        <button className="button-38" type="button" 
                    style={{ justifyContent: 'center'}}
                    onClick={addTrade}>Add a Trade</button>
        </div> )};

