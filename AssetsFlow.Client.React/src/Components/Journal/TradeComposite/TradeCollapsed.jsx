import React, { useState, useEffect } from "react";
import * as Constants from "@constants/journalConstants";
import TradeElement from "@journalComponents/TradeElement/TradeElement";

function TradeCollapsed({ trade }) {
  const [simulatedEle, setSimulatedEle] = useState(null);

  useEffect(() => {
    // Generate a simulated trade element
    const simulatedEntries = [
      ...trade.tradeElements,
      ...(trade[Constants.TRADE_SUMMARY_STRING] ? [trade[Constants.TRADE_SUMMARY_STRING]] : [])
    ]
      .flatMap((tradeElement) =>
        tradeElement[Constants.TRADE_ENTRIES_STRING].filter(
          (entry) => entry[Constants.DATA_RELEVANT_FOR_ORVERVIEW_STRING]
        )
      );

    const simulatedElement = {
      id: `Simulated-${trade.id}`, // Assign a unique ID based on tradeId
      isOverview: true, // Mark this as a simulated element
      entries: simulatedEntries, // Add the simulated entries array
    };

    setSimulatedEle(simulatedElement);
  }, []);

  return (
    <>
      {simulatedEle ? (<TradeElement tradeElement={simulatedEle} />) : (
        <div>Loading...</div>
      )}
    </>
  );
}

export default React.memo(TradeCollapsed);
