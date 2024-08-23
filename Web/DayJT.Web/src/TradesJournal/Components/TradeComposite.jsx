import {React, memo} from 'react';
import TradeElement from './TradeElement';
import { useQuery, useQueryClient } from '@tanstack/react-query'
import * as Constants from '../../Constants/constants'
import * as TradesApiAccess from './../Services/TradesApiAccess'

export default function TradeComposite({tradeComposite}) {
    
   const tradeElementsValue = tradeComposite[Constants.TRADE_ELEMENTS_KEY]

    const summaryQueryKey = [Constants.TRADECOMPOSITE_SUMMARY_KEY, tradeComposite.Id]
    const queryClient = useQueryClient()

    const { data: tradeSummary } = useQuery({
        queryKey: summaryQueryKey,
        initialData: tradeComposite.Summary,
        refetchOnWindowFocus: false,
        queryFn: TradesApiAccess.getSummaryElement
      })

    const onElementUpdate = (data) => {
        const { updatedEntry, newSummary } = data;

        const clientIdValue = tradeComposite[Constants.TRADE_CLIENTID_KEY]
        queryClient.setQueryData([Constants.TRADES_KEY, clientIdValue], (oldTradeComposite) => 
        produce(oldTradeComposite, draft => {
            const tradeElementsValue = draft[Constants.TRADE_ELEMENTS_KEY]
            for (const tradeElement of tradeElementsValue) {
                const entryIndex = tradeElement.entries.findIndex(entry => entry.id === updatedEntry.id);
                if (entryIndex !== -1) {
                    // Update the specific Entry
                    draft.tradeElements[tradeElement.id].Entries[entryIndex] = updatedEntry;
                    break;
                }
            }
        })
      );
      
      //update summary, internal so we can update just here
      queryClient.setQueryData(summaryQueryKey, data)
    }

    return (
        <div id="tradeComposite">
            <ul>
                {tradeElementsValue.map(ele=> (
                    <li key={ele.id}>
                        <TradeElement tradeElement={ele} onElementUpdate={onElementUpdate}/>
                    </li>
                    ))}
            </ul>
            {data ?  <TradeElement tradeStep={ tradeSummary }/> : ''}  
        </div> )};