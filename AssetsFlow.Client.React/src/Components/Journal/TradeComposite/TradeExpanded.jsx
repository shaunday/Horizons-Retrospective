import React from "react";
import * as Constants from "@constants/journalConstants";
import CompositeControls from "./Controls/CompositeControls";
import { useTradeStateAndManagement } from "@hooks/Journal/Composite/useTradeStateAndManagement";
import { newStatesResponseParser } from "@services/newStatesResponseParser"
import TradeElementWrapper from "../TradeElement/TradeElementWrapper";
import TradeElementCollapsed from "../TradeElement/TradeElementCollapsed";

function TradeExpanded({ trade }) {
  const { tradeSummary, processEntryUpdate, processTradeAction, processSummaryUpdate } =
    useTradeStateAndManagement(trade);

  const processElementAction = (action, response) => {
    const { newSummary } = newStatesResponseParser(response);
    processSummaryUpdate(newSummary);
  };

  return (
    <div className="my-2">
      <ul className="flex flex-col gap-2">
        {trade[Constants.TRADE_ELEMENTS_STRING].map((ele) => (
          <li key={ele.id}>
            <TradeElementWrapper
              tradeElement={ele}
              onElementContentUpdate={processEntryUpdate}
              onElementAction={processElementAction}
            />
          </li>
        ))}
      </ul>

      {tradeSummary && (
        <div className="mt-3">
          <TradeElementCollapsed tradeElement={tradeSummary} />
        </div>
      )}

      {trade[Constants.TRADE_STATUS] !== Constants.TradeStatus.CLOSED && (
        <CompositeControls
          tradeComposite={trade}
          onTradeActionSuccess={processTradeAction}
        />
      )}
    </div>
  );
}

export default React.memo(TradeExpanded);