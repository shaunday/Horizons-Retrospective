import {React, memo} from 'react';
import TradeElement from './TradeElement';
import { useQuery  } from '@tanstack/react-query'
import * as Constants from '../../Constants/constants'

export default function TradeComposite({tradeComposite}) {
    
    const { data: tradeSummary } = useQuery({
        queryKey: [Constants.TRADECOMPOSITE_SUMMARY_KEY, tradeComposite.Id],
        initialData: tradeComposite.Summary,
        queryFn: async () => await TradesApiAccess.getAllTrades().data //todo
      })

      const onElementUpdate = (data) => {
        //set summary
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