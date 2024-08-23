import React, { memo } from 'react';
import TradeElement from './TradeElement';
import { useTradeUpdate } from '@hooks/useTradeUpdate';
import { useTradeSummary } from '@hooks/useTradeSummary';
import * as Constants from '@constants/constants';

const TradeComposite = memo(({ tradeComposite }) => {
    const tradeElementsValue = tradeComposite[Constants.TRADE_ELEMENTS_KEY];
    
    const tradeSummary = useTradeSummary(tradeComposite);
    const { onElementUpdate } = useTradeUpdate(tradeComposite);

    return (
        <div id="tradeComposite">
            <ul>
                {tradeElementsValue.map(ele => (
                    <li key={ele.id}>
                        <TradeElement tradeElement={ele} onElementUpdate={onElementUpdate} />
                    </li>
                ))}
            </ul>
            {tradeSummary ? <TradeElement tradeStep={tradeSummary} /> : ''}
        </div>
    );
});

export default TradeComposite;
