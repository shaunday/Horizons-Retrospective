import React, { useMemo } from "react";
import * as Constants from "@constants/journalConstants";
import ElementControls from "./ElementControls";
import GroupedEntriesList from "../DataElementGroups/GroupedEntriesList";

function TradeElementExpanded({ tradeElement }) {
  const isAllowControls = useMemo(() => {
    const elementType = tradeElement[Constants.ELEMENT_TYPE_STING];
    return (
      elementType !== Constants.ElementType.ORIGIN &&
      elementType !== Constants.ElementType.SUMMARY
    );
  }, [tradeElement]);

  return (
    <div className="flex items-center">
      <GroupedEntriesList
        entries={tradeElement[Constants.TRADE_ENTRIES_STRING]}
      />
      {isAllowControls && (
        <ElementControls className="m-3" tradeElement={tradeElement} />
      )}
    </div>
  );
}

export default React.memo(TradeElementExpanded);
