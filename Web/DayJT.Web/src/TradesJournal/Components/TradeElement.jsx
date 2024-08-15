import React from 'react';
import Cell from './Cell';
import { useQuery  } from '@tanstack/react-query'
import * as Constants from '../../Constants/constants'

export default function TradeElement({tradeElement, onElementUpdate}) {

    const { data: tradeEle } = useQuery({
        queryKey: [Constants.TRADE_ELEMENT_KEY, tradeElement.Id],
        initialData: tradeElement,
        queryFn: async () => await TradesApiAccess.getAllTrades().data //todo
      })

      const onCellUpdate = (data) => {
        onElementUpdate(data);
      }

    return (
        <div id="tradeElement">
            <ul>
                {tradeEle.Entries.map(entry=> (
                    <li key={entry.id}>
                        <Cell cellInfo={entry} onCellUpdate={onCellUpdate}/>
                    </li>
                    ))}
             </ul>
        </div>)};