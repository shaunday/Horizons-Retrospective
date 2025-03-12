import React, { useState, useCallback } from "react";
import { ActionIcon } from "@mantine/core";
import { TbChevronDownRight, TbChevronUpLeft } from 'react-icons/tb';
import TradeExpanded from "./TradeExpanded";
import TradeCollapsed from "./TradeCollapsed";

function TradeWrapper({ tradeId }) {
  const [isCollapsed, setIsCollapsed] = useState(true);

  const toggleExpand = useCallback(() => {
    setIsCollapsed((prev) => !prev);
  }, []);

  return (
    <div style={{ display: "flex", alignItems: "center", height: "100%" }}>
      <ActionIcon variant="subtle" onClick={toggleExpand} style={{ height: "50px", marginRight: "3px" }}>
        {isCollapsed ? <TbChevronDownRight /> : <TbChevronUpLeft />}
      </ActionIcon>
      {isCollapsed ? (
        <TradeCollapsed tradeId={tradeId} />
      ) : (
        <TradeExpanded tradeId={tradeId} />
      )}
    </div>
  );
}

export default React.memo(TradeWrapper);