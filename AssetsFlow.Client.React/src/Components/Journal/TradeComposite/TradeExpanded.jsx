import React from "react";
import * as Constants from "@constants/journalConstants";
import CompositeControls from "./Controls/CompositeControls";
import { useTradeStateAndManagement } from "@hooks/Journal/Composite/useTradeStateAndManagement";
import TradeElementBadge from "../TradeElement//TradeElementBadge";
import { newStatesResponseParser } from "@services/newStatesResponseParser"
import TradeCollapsed from "./TradeCollapsed";
import TradeElementWrapper from "../TradeElement/TradeElementWrapper";
import TradeElementCollapsed from "../TradeElement/TradeElementCollapsed";

const styles = {
  listItem: {
    padding: "10px",
    border: "1.5px solid purple",
    borderRadius: "6px",
    margin: "8px 4px",
    position: "relative",
  },
};

function TradeExpanded({ trade }) {
  const { tradeSummary, processEntryUpdate, processTradeAction, processSummaryUpdate } =
    useTradeStateAndManagement(trade);

  const processElementAction = (action, response) => {
    const { newSummary } = newStatesResponseParser(response);
    processSummaryUpdate(newSummary);
  };

  return (
    <div>
      <ul style={{ flexDirection: "column" }}>
        {trade[Constants.TRADE_ELEMENTS_STRING].map((ele) => (
          <li key={ele.id} style={styles.listItem}>
            <TradeElementBadge tradeElement={ele} />
            <TradeElementWrapper
              tradeElement={ele}
              onElementContentUpdate={processEntryUpdate}
              onElementAction={processElementAction}
            />
          </li>
        ))}
      </ul>
      {tradeSummary && <TradeElementCollapsed tradeElement={tradeSummary}/>}
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