import React, { useMemo } from "react";
import * as Constants from "@constants/journalConstants";
import EntriesList from "../DataElementGroups/EntriesList";
import { getOverViewEntries } from "@services/getOverViewEntries";
import Notifications from "@components/Notifications";

function TradeElementCollapsed({ tradeElement }) {
    const entriesForLocalOverview = useMemo(() => {
        return getOverViewEntries(
          [tradeElement],
          Constants.DATA_RELEVANT_FOR_LOCAL_ORVERVIEW_STRING
        );
      }, [tradeElement]);
      

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