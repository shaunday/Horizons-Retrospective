import React, { useState } from "react";
import * as Constants from "@constants/journalConstants";
import TradeElement from "@journalComponents/TradeElement";
import { useTrade } from "@hooks/useTrade";

function TradeCollapsed({ tradeId }) {
  const { trade } = useTrade(tradeId);
  const [simulatedEle, setSimulatedEle] = useState(null);

  useEffect(() => {
    // Generate a simulated trade element
    const simulatedElement = trade.tradeElements.map((tradeElement) => {
      // Filter Entries based on the relevant property
      const relevantEntries = tradeElement.Entries?.filter(
        (entry) => entry[Constants.RELEVANT_FOR_ORVERVIEW_STRING]
      );

      return {
        id: `Simulated-${tradeElement.id}`, // Assign a new ID or unique property
        simulated: true, // Mark this as a simulated element
        Entries: relevantEntries,
      };
    });

    // Set the simulated trade element in state
    setSimulatedEle(simulatedElement);
  }, [trade]);
  
  return (
    <>
       <TradeElement
              tradeElement={simulatedEle}
              style={index !== 0 ? { marginTop: "10px" } : {}}
            />
    </>
  );
}

export default React.memo(TradeCollapsed);
