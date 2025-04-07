import React, { useState } from "react";
import { ActionIcon, Stack } from "@mantine/core";
import { TbChevronDownRight, TbChevronUpLeft } from "react-icons/tb";
import TradeExpanded from "./TradeExpanded";
import TradeCollapsed from "./TradeCollapsed";
import { useGetTradeById } from "@hooks/Journal/useGetTradeById";
import TradeCompositeBadge from "./Badge/TradeCompositeBadge";

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
};

function TradeWrapper({ tradeId }) {
  const { trade } = useGetTradeById(tradeId);
  const [isCollapsed, setIsCollapsed] = useState(true);

  if (!trade) {
    return <div>Loading...</div>; 
  }

  return (
    <div style={styles.tradeItem}>
      <Stack gap="xs" style={{ margin: "0 7px 0 1px", alignItems: "center" }}>
        <TradeCompositeBadge tradeComposite={trade} />
        <ActionIcon
          variant="subtle"
          onClick={() => setIsCollapsed((prev) => !prev)}
          style={{ ...(isCollapsed ? {} : { height: 50 }) }}
        >
           {isCollapsed ? <TbChevronDownRight size={22} /> : <TbChevronUpLeft size={22} style={{ height: "50px" }} />}
        </ActionIcon>
      </Stack>
      {isCollapsed ? <TradeCollapsed trade={trade} /> : <TradeExpanded trade={trade} />}
    </div>
  );
}

export default React.memo(TradeWrapper);