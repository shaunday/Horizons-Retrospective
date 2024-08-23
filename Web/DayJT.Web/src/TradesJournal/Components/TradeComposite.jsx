import React, { useState } from 'react';
import TradeElement from './TradeElement';
import { useTradeUpdate } from '@hooks/useTradeUpdate';
import * as Constants from '@constants/constants';

const TradeComposite = (({ tradeComposite }) => {
    const initialTradeSummaryValue = tradeComposite[Constants.TRADE_SUMMARY_STRING];
    const initialElementsValue = tradeComposite[Constants.TRADE_ELEMENTS_STRING];

    const [tradeSummary, setTradeSummary] = useState(initialTradeSummaryValue);

    const entryUpdate = useTradeUpdate(tradeComposite);
    const onEntryUpdate = ({updatedEntry, newSummary }) => {
        entryUpdate(updatedEntry);
        setTradeSummary(newSummary);
    }

    return (
        <div id="tradeComposite">
            <ul>
                {initialElementsValue.map(ele => (
                    <li key={ele.id}>
                        <TradeElement tradeElement={ele} onElementUpdate={onEntryUpdate} />
                    </li>
                ))}
            </ul>
            {tradeSummary ? <TradeElement tradeStep={tradeSummary} /> : ''}
        </div>
    );
});

export default TradeComposite;
