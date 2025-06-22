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
      

    return (
        <div className="flex items-center justify-between w-full">
          <EntriesList entries={entriesForLocalOverview} overviewType={Constants.OverviewType.ElementOverview}/>
          {false && (
            <Notifications 
              shortText="Missing" 
              expandedText="This element has missing required fields." 
            />
          )}
        </div>
    );
}

export default React.memo(TradeElementCollapsed);