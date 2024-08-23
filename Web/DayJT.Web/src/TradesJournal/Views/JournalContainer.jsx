import React from 'react'
import FilterControl from '@components/FilterControl'
import TradeComposite from '@components/TradeComposite';
import { useTrades } from '@hooks/useTrades'

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

