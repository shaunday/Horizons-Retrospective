import React from 'react'
import FilterControl from './FilterControl'
import Trades from './Trades';
import * as TradesApiAccess from './../Services/TradesApiAccess'
import useAllTrades from './../useAllTrades'

function JournalContainer() {
    const [allTrades, setAllTrades] = React.useState(null);

    React.useEffect(() => { setAllTrades(TradesApiAccess.getAllTrades())} , []);

    return (
        <div id="journalMainBody">
        <FilterControl/>
        <Trades/>
        <button className="button-38" type="button" 
                    style={{ justifyContent: 'center'}}>Add a Trade</button>
        </div> )};


export default JournalContainer;