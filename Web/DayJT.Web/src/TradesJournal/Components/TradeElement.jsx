import React from 'react';
import Cell from './Cell';
import { useQuery, useQueryClient } from '@tanstack/react-query'
import * as Constants from '../../Constants/constants'
import * as TradesApiAccess from './../Services/TradesApiAccess'

export default function TradeElement({tradeElement, onElementUpdate}) {

    const elementQueryKey = [Constants.TRADE_ELEMENT_KEY, tradeElement.TradeCompositeRefId, tradeElement.Id]
    const queryClient = useQueryClient()

    const { data: tradeEle } = useQuery({
        queryKey: elementQueryKey,
        initialData: tradeElement,
        refetchOnWindowFocus: false,
        queryFn: TradesApiAccess.getElement 
      })

      const onCellUpdate = (data) => {
        queryClient.invalidateQueries({ queryKey: elementQueryKey }) //cells within an element might have inter-relations
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