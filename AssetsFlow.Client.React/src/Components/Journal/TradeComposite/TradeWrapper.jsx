import React, { useState } from "react";
import { ActionIcon, Paper } from "@mantine/core";
import { TbChevronDownRight, TbChevronUpLeft } from "react-icons/tb";
import TradeExpanded from "./TradeExpanded";
import TradeCollapsed from "./TradeCollapsed";
import { useGetTradeById } from "@hooks/Journal/useGetTradeById";
import TradeCompositeBadge from "./Badge/TradeCompositeBadge";

function TradeWrapper({ tradeId, indexType }) {
  const { trade } = useGetTradeById(tradeId);
  const [isCollapsed, setIsCollapsed] = useState(true);

  if (!trade) {
    return <div>Loading...</div>;
  }

  const badgeActionWrapperStyle = {
    display: "flex",
    flexDirection: isCollapsed ? "row" : "column",
    gap: "4px",
    marginRight: "10px"
  };

  const tradeItemStyle = {
    padding: "5px",
    marginBottom: "5px",
    display: "flex",
    alignItems: "center",
    height: "100%",
    background: indexType === 1 ? "rgb(234, 237, 221)" : "rgb(219, 240, 236)"
  };

  return (
    <Paper shadow="md" style={tradeItemStyle} withBorder>
      <div style={badgeActionWrapperStyle}>
        <ActionIcon
          variant="subtle"
          onClick={() => setIsCollapsed((prev) => !prev)}
          style={{ ...(isCollapsed ? {} : { height: 50 }) }}
        >
          {isCollapsed ? (
            <TbChevronDownRight size={22} />
          ) : (
            <TbChevronUpLeft size={22} style={{ height: "50px" }} />
          )}
        </ActionIcon>
        {isCollapsed && <TradeCompositeBadge tradeComposite={trade} />}
      </div>
      {isCollapsed ? <TradeCollapsed trade={trade} /> : <TradeExpanded trade={trade} />}
    </Paper>
  );
}

export default React.memo(TradeWrapper);
