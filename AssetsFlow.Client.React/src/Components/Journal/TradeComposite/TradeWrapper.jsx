import React, { useState, useCallback } from "react";
import { ActionIcon } from "@mantine/core";
import { IconChevronDown, IconChevronUp } from '@tabler/icons-react';
import TradeExpanded from "./TradeExpanded";
import TradeCollapsed from "./TradeCollapsed";

function TradeWrapper({ tradeId }) {
  const [isCollapsed, setIsCollapsed] = useState(true);

  const toggleExpand = useCallback(() => {
    setIsCollapsed((prev) => !prev);
  }, []);

  return (
    <div style={{ display: "flex", alignItems: "center", height: "100%" }}>
      <ActionIcon variant="subtle" onClick={toggleExpand} style={{ height: "50px" }}>
        {isCollapsed ? <IconChevronDown /> : <IconChevronUp />}
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