import React from "react";
import * as Constants from "@constants/journalConstants";
import CompositeControls from "./Controls/CompositeControls";
import { useTradeStateAndManagement } from "@hooks/Journal/Composite/useTradeStateAndManagement";
import { newStatesResponseParser } from "@services/newStatesResponseParser"
import TradeElementWrapper from "../TradeElement/TradeElementWrapper";
import TradeElementCollapsed from "../TradeElement/TradeElementCollapsed";
import { TbChartBar } from "react-icons/tb";
import clsx from "clsx";

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

      <div className="mt-3 flex items-center justify-between">
        <div className="flex items-center gap-2">
          {tradeSummary && (
            <>
              <TbChartBar size={16} className="text-slate-500" />
              <TradeElementCollapsed tradeElement={tradeSummary} />
            </>
          )}
        </div>
        
        {trade[Constants.TRADE_STATUS] !== Constants.TradeStatus.CLOSED && (
          <div className="mr-7">
            <CompositeControls
              tradeComposite={trade}
              onTradeActionSuccess={processTradeAction}
            />
          </div>
        )}
      </div>
    </div>
  );
}

export default React.memo(TradeExpanded);