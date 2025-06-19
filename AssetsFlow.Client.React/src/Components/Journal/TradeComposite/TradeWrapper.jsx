import React, { useState } from "react";
import { ActionIcon, Paper } from "@mantine/core";
import { TbChevronDownRight, TbChevronUpLeft } from "react-icons/tb";
import TradeExpanded from "./TradeExpanded";
import TradeCollapsed from "./TradeCollapsed";
import { useGetTradeById } from "@hooks/Journal/useGetTradeById";
import TradeCompositeBadge from "./Badge/TradeCompositeBadge";
import TradeNotifications from "./Controls/TradeNotifications";
import TradeAdminControls from "./Controls/TradeAdminControls";

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
    marginRight: isCollapsed? 10 : 15,
  };

  const tradeItemStyle = {
    padding: "5px",
    marginBottom: "5px",
    display: "flex",
    alignItems: "center",
    height: "100%",
    justifyContent: "space-between",
    background: isCollapsed? "none" : indexType === 1 ? "rgb(241, 242, 235)" : "rgb(231, 242, 235)"
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
      <div style={{display: "flex", justifyContent: "flex-end"}}>
        {isCollapsed && <TradeNotifications tradeComposite={trade} />}
        {/* <TradeAdminControls trade={trade} /> */}
      </div>

    </Paper>
  );
}

export default React.memo(TradeWrapper);
