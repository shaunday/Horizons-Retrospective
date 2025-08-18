import React from "react";
import * as Constants from "@constants/journalConstants";
import CompositeControls from "./Controls/CompositeControls";
import TradeElementWrapper from "../TradeElement/TradeElementWrapper";
import TradeElementCollapsed from "../TradeElement/TradeElementCollapsed";
import { TbChartBar } from "react-icons/tb";

// eslint-disable-next-line react/prop-types
function TradeExpanded({ trade }) {
  return (
    <div className="my-2">
      <ul className="flex flex-col gap-2">
        {trade[Constants.TRADE_ELEMENTS_STRING].map((ele) => (
          <li key={ele.id}>
            <TradeElementWrapper tradeElement={ele} />
          </li>
        ))}
      </ul>

      <div className="mt-3 flex items-center justify-between">
        <div className="flex items-center gap-2">
          {trade[Constants.TRADE_SUMMARY_STRING] && (
            <>
              <TbChartBar size={16} className="text-slate-500" />
              <TradeElementCollapsed
                tradeElement={trade[Constants.TRADE_SUMMARY_STRING]}
                isUseAllEntries={true}
              />
            </>
          )}
        </div>

        {trade[Constants.TRADE_STATUS] !== Constants.TradeStatus.CLOSED && (
          <div className="mr-7">
            <CompositeControls tradeComposite={trade} />
          </div>
        )}
      </div>
    </div>
  );
}

export default React.memo(TradeExpanded);
