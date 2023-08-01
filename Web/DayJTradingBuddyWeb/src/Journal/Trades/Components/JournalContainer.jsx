import React from 'react'
import FilterControl from './FilterControl'
import Trades from './Trades';

function JournalContainer() {
    return (
        <div id="journalMainBody">
        <FilterControl/>
        <Trades/>
        <button className="button-38" type="button" 
                    style={{ justifyContent: 'center'}}>Add a Trade</button>
        </div> )};


export default JournalContainer;