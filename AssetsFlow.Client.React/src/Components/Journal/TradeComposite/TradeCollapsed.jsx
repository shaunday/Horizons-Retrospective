import React, { useMemo } from "react";
import * as Constants from "@constants/journalConstants";
import { getOverViewEntries } from "@services/getOverViewEntries";
import EntriesList from "../DataElementGroups/EntriesList";
import Notifications from "@components/Notifications";

function TradeCollapsed({ trade }) {
  const EntriesForTradeOverView = useMemo(() => {
    if (!trade) return [];

    const elementsToFlatten = [
      ...trade[Constants.TRADE_ELEMENTS_STRING],
      ...(trade[Constants.TRADE_SUMMARY_STRING] ? [trade[Constants.TRADE_SUMMARY_STRING]] : []),
    ];

    return getOverViewEntries(elementsToFlatten, Constants.DATA_RELEVANT_FOR_ORVERVIEW_STRING);
  }, [trade]);

  if (!trade) {
    return <div>Loading...</div>;
  }

  const hasMissingContent = trade[Constants.HasMissingContent];

  return (
    <div className="flex items-center justify-between w-full">
      <EntriesList
        entries={EntriesForTradeOverView}
        overviewType={Constants.OverviewType.TRADE_OVERVIEW}
      />
      {hasMissingContent && (
        <Notifications
          shortText="Missing"
          expandedText="Some required fields are empty or invalid."
        />
      )}
    </div>
  );
}

export default React.memo(TradeCollapsed);
