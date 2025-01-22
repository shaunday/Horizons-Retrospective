import React, { useState, useCallback } from "react";
import * as Constants from "@constants/journalConstants";
import TradeExpanded from "./TradeExpanded";
import TradeCollapsed from "./TradeCollapsed";

function TradeWrapper({ tradeId }) {
  const [isCollapsed, setIsCollapsed] = useState(false);

  const toggleCollapse = useCallback(() => {
    setIsCollapsed((prev) => !prev);
  }, []);

  return (
    <div onClick={toggleCollapse}>
      <div>{isCollapsed ? "▼" : "▲"}</div>
      {isCollapsed ? (
        <TradeCollapsed tradeId={tradeId} />
      ) : (
        <TradeExpanded tradeId={tradeId} />
      )}
    </div>
  );
}

export default React.memo(TradeWrapper);
