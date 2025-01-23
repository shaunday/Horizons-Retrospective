import React, { useState, useEffect } from "react";
import * as Constants from "@constants/journalConstants";
import TradeElement from "@journalComponents/TradeElement";
import { useTrade } from "@hooks/useTrade";

function TradeCollapsed({ tradeId }) {
  const { trade } = useTrade(tradeId);
  const [simulatedEle, setSimulatedEle] = useState(null);

  useEffect(() => {
    // Generate a simulated trade element
    
    const simulatedEntries = trade.tradeElements
    .flatMap((tradeElement) => {
        return tradeElement[Constants.TRADE_ENTRIES_STRING].filter(
            (entry) => entry[Constants.RELEVANT_FOR_ORVERVIEW_STRING]);
        })

    const simulatedElement = {
      id: `Simulated-${tradeId}`, // Assign a unique ID based on tradeId
      simulated: true, // Mark this as a simulated element
      entries: simulatedEntries, // Add the simulated entries array
    };

    setSimulatedEle(simulatedElement);
  }, []);

  return (
    <>
       {simulatedEle ? ( <TradeElement tradeElement={simulatedEle} />) : (
        <div>Loading...</div>
        )}
    </>
  );
}

export default React.memo(TradeCollapsed);
