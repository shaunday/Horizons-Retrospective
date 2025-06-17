import React, { useMemo } from "react";
import * as Constants from "@constants/journalConstants";
import EntriesList from "../DataElementGroups/EntriesList";
import { getOverViewEntries } from "@services/getOverViewEntries";

function TradeElementCollapsed({ tradeElement }) {
    const entriesForLocalOverview = useMemo(() => {
        return getOverViewEntries(
          [tradeElement],
          Constants.DATA_RELEVANT_FOR_LOCAL_ORVERVIEW_STRING
        );
      }, [tradeElement]);
      

    return (
        <>
          <EntriesList entries={entriesForLocalOverview} overviewType={Constants.OverviewType.ElementOverview}/>
        </>
    );
}

export default React.memo(TradeElementCollapsed);