import {React, memo} from 'react';
import TradeElement from './TradeElement';
import { useQuery, useQueryClient } from '@tanstack/react-query'
import * as Constants from '../../Constants/constants'
import * as TradesApiAccess from './../Services/TradesApiAccess'

export default function TradeComposite({tradeComposite}) {
    
    const summaryQueryKey = [Constants.TRADECOMPOSITE_SUMMARY_KEY, tradeComposite.Id]
    const queryClient = useQueryClient()

    const { data: tradeSummary } = useQuery({
        queryKey: summaryQueryKey,
        initialData: tradeComposite.Summary,
        refetchOnWindowFocus: false,
        queryFn: TradesApiAccess.getSummaryElement
      })

      const onElementUpdate = (data) => {
        queryClient.setQueryData(summaryQueryKey, data)
      }

    return (
        <div id="tradeComposite">
            <ul>
                {tradeComposite.TradeElements.map(ele=> (
                    <li key={ele.id}>
                        <TradeElement tradeElement={ele} onElementUpdate={onElementUpdate}/>
                    </li>
                    ))}
            </ul>
            {data ?  <TradeElement tradeStep={ tradeSummary }/> : ''}  
        </div> )};