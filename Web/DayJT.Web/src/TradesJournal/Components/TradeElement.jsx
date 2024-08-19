import React from 'react';
import Cell from './Cell';
import { useQuery  } from '@tanstack/react-query'
import * as Constants from '../../Constants/constants'
import * as TradesApiAccess from './../Services/TradesApiAccess'

export default function TradeElement({tradeElement, onElementUpdate}) {

    const { data: tradeEle } = useQuery({
        queryKey: [Constants.TRADE_ELEMENT_KEY, tradeElement.TradeCompositeRefId, tradeElement.Id],
        initialData: tradeElement,
        refetchOnWindowFocus: false,
        queryFn: TradesApiAccess.getElement //todo
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