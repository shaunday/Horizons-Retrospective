import React, { useMemo } from "react";
import { Group } from "@mantine/core";
import * as Constants from "@constants/journalConstants";
import { getOverViewEntries } from "@services/getOverViewEntries"
import EntriesList from "../DataElementGroups/EntriesList";

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
     <Group wrap="wrap" spacing={10}>
      <EntriesList entries={EntriesForTradeOverView} overviewType={Constants.OverviewType.TRADE_OVERVIEW} />
      {/* Notifications here */}
    </Group>
  );
}

export default React.memo(TradeCollapsed);
