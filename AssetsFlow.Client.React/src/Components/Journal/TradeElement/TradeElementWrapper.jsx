import React, { useState } from "react";
import { ActionIcon } from "@mantine/core";
import { TbChevronRight, TbChevronLeft } from "react-icons/tb";
import TradeElementExpanded from "./TradeElementExpanded";
import TradeElementCollapsed from "./TradeElementCollapsed";
import TradeElementBadge from "./Badge/TradeElementBadge";

const styles = {
  elementItem: {
    display: "flex",
    alignItems: "center",
    height: "100%",
    padding: "10px 10px 10px 30px", 
    border: "1.5px solid purple",
    borderRadius: "6px",
    position: "relative",
  },
};

function TradeElementWrapper({ tradeElement, onElementContentUpdate, onElementAction }) {
  const [isCollapsed, setIsCollapsed] = useState(true);

  const badgeStyle = {
    position: "absolute",
    left: "-12px",
    top: "50%",
    transform: "translateY(-50%)",
    borderRadius: "0 6px 6px 0",
    zIndex: 1,
  };

  const chevronAndContentStyle = {
    display: "flex",
    flexDirection: isCollapsed ? "row" : "column",
    gap: "4px",
    marginRight: "5px",
  };

  return (
    <div style={styles.elementItem}>
      <div style={badgeStyle}>
        <TradeElementBadge tradeElement={tradeElement} />
      </div>

      <div style={chevronAndContentStyle}>
        <ActionIcon
          variant="subtle"
          onClick={() => setIsCollapsed((prev) => !prev)}
          style={{ ...(isCollapsed ? {} : { height: 50 }) }}
        >
          {isCollapsed ? (
            <TbChevronRight size={22} />
          ) : (
            <TbChevronLeft size={22} style={{ height: "50px" }} />
          )}
        </ActionIcon>
      </div>

      {/* Render content */}
      {isCollapsed ? (
        <TradeElementCollapsed tradeElement={tradeElement} isTradeOverview={false} />
      ) : (
        <TradeElementExpanded
          tradeElement={tradeElement}
          onElementContentUpdate={onElementContentUpdate}
          onElementAction={onElementAction}
        />
      )}
    </div>
  );
}

export default React.memo(TradeElementWrapper);
