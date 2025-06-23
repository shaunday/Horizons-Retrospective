import React, { useMemo } from "react";
import * as Constants from "@constants/journalConstants";
import EntriesList from "../DataElementGroups/EntriesList";
import { getOverViewEntries } from "@services/getOverViewEntries";
import Notifications from "@components/Notifications";

function TradeElementCollapsed({ tradeElement, isUseAllEntries = false }) {
    const entriesForLocalOverview = useMemo(() => {
        if (isUseAllEntries) {
          return tradeElement[Constants.TRADE_ENTRIES_STRING];
        }
        return getOverViewEntries(
          [tradeElement],
          Constants.DATA_RELEVANT_FOR_LOCAL_ORVERVIEW_STRING
        );
      }, [tradeElement, isUseAllEntries]);
      const hasMissingContent = tradeElement[Constants.HasMissingContent];
      

    return (
        <div className="flex items-center justify-between w-full gap-1">
          <EntriesList entries={entriesForLocalOverview} overviewType={Constants.OverviewType.ElementOverview}/>
          {hasMissingContent && (
            <Notifications 
              shortText="" 
              expandedText="This element has missing required fields." 
            />
          )}
        </div>
    );
}

export default React.memo(TradeElementCollapsed);