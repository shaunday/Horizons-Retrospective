import React, { useState, useCallback } from "react";
import { ActionIcon } from "@mantine/core";
import { TbChevronDownRight, TbChevronUpLeft } from "react-icons/tb";
import TradeExpanded from "./TradeExpanded";
import TradeCollapsed from "./TradeCollapsed";
import { useGetTradeById } from "@hooks/Journal/useGetTradeById";

const styles = {
  tradeItem: {
    border: "1.5px solid blue",
    padding: "5px",
    borderRadius: "4px",
    marginBottom: "5px",
    display: "flex",
    alignItems: "center",
    height: "100%",
  },
  actionIcon: {
    height: "50px",
    margin: "0 7px 0 1px",
  },
};

function TradeWrapper({ tradeId }) {
  const { trade } = useGetTradeById(tradeId);
  const [isCollapsed, setIsCollapsed] = useState(true);

  const toggleExpand = useCallback(() => {
    setIsCollapsed((prev) => !prev);
  }, []);

  return (
    <div style={styles.tradeItem}>
      <ActionIcon variant="subtle" onClick={toggleExpand} style={styles.actionIcon}>
        {isCollapsed ? <TbChevronDownRight size={22} /> : <TbChevronUpLeft size={22} />}
      </ActionIcon>
      {isCollapsed ? <TradeCollapsed trade={trade} /> : <TradeExpanded trade={trade} />}
    </div>
  );
}

export default React.memo(TradeWrapper);