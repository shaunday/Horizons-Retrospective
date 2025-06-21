import React, { useMemo } from "react";
import { Group } from "@mantine/core";
import * as Constants from "@constants/journalConstants";
import { getOverViewEntries } from "@services/getOverViewEntries"
import EntriesList from "../DataElementGroups/EntriesList";
import TradeNotifications from "./Controls/TradeNotifications";

function TradeCollapsed({ trade }) {
  if (!trade) {
    return <div>Loading...</div>; 
  }

  const EntriesForTradeOverView = useMemo(() => {
    const elementsToFlatten = [
      ...trade[Constants.TRADE_ELEMENTS_STRING],
      ...(trade[Constants.TRADE_SUMMARY_STRING] ? [trade[Constants.TRADE_SUMMARY_STRING]] : []),
    ];

    return getOverViewEntries(
      elementsToFlatten,
      Constants.DATA_RELEVANT_FOR_ORVERVIEW_STRING
    );
  }, [trade]);

   return (
     <div className="flex items-center justify-between w-full">
       <EntriesList entries={EntriesForTradeOverView} overviewType={Constants.OverviewType.TRADE_OVERVIEW} />
       <TradeNotifications tradeComposite={trade} />
     </div>
   );
}

export default React.memo(TradeCollapsed);
