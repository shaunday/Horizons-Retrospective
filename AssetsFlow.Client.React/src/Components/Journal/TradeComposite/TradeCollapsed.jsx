import React, { useState, useEffect } from "react";
import * as Constants from "@constants/journalConstants";
import TradeElementContainer from "@journalComponents/TradeElement/TradeElementContainer";
import { useGetTradeById } from "@hooks/useGetTradeById";

function TradeCollapsed({ tradeId }) {
  const { trade } = useGetTradeById(tradeId);
  const [simulatedEle, setSimulatedEle] = useState(null);

  useEffect(() => {
    // Generate a simulated trade element
    
    const simulatedEntries = trade.tradeElements
    .flatMap((tradeElement) => {
        return tradeElement[Constants.TRADE_ENTRIES_STRING].filter(
            (entry) => entry[Constants.DATA_RELEVANT_FOR_ORVERVIEW_STRING]);
        })

    const simulatedElement = {
      id: `Simulated-${tradeId}`, // Assign a unique ID based on tradeId
      isOverview: true, // Mark this as a simulated element
      entries: simulatedEntries, // Add the simulated entries array
    };

    setSimulatedEle(simulatedElement);
  }, []);

  return (
    <>
       {simulatedEle ? ( <TradeElementContainer tradeElement={simulatedEle} />) : (
        <div>Loading...</div>
        )}
    </>
  );
}

export default React.memo(TradeCollapsed);
