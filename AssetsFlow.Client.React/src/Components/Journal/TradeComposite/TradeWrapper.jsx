import React, { useState } from "react";
import { ActionIcon } from "@mantine/core";
import { TbChevronDownRight, TbChevronUpLeft } from "react-icons/tb";
import TradeExpanded from "./TradeExpanded";
import TradeCollapsed from "./TradeCollapsed";
import { useGetTradeById } from "@hooks/Journal/useGetTradeById";
import TradeCompositeBadge from "./Badge/TradeCompositeBadge";

const styles = {
  tradeItem: {
    border: "1.5px solid #ccc",
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

  const badgeActionWrapperStyle = {
    display: "flex",
    flexDirection: isCollapsed ? "row" : "column",
    gap: "4px",
    marginRight: "5px"
  };

  return (
    <div style={styles.tradeItem}>
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
    </div>
  );
}

export default React.memo(TradeWrapper);
