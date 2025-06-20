import React, { useState } from "react";
import { ActionIcon, Paper } from "@mantine/core";
import { TbChevronRight, TbChevronLeft } from "react-icons/tb";
import TradeElementExpanded from "./TradeElementExpanded";
import TradeElementCollapsed from "./TradeElementCollapsed";
import TradeElementBadge from "./Badge/TradeElementBadge";

const styles = {
  elementItem: {
    display: "flex",
    alignItems: "center",
    height: "100%",
    background:" rgba(224, 208, 221, 0.32)",
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
  };

  return (
    <Paper style={styles.elementItem} className="py-1 px-2.5 pl-6">
      <div style={badgeStyle}>
        <TradeElementBadge tradeElement={tradeElement} />
      </div>

      <div style={chevronAndContentStyle} className="mr-1">
        <ActionIcon
          variant="subtle"
          onClick={() => setIsCollapsed((prev) => !prev)}
          style={{ ...(isCollapsed ? {} : { height: 50 }) }}
          className={!isCollapsed ? "mr-2" : ""}
        >
          {isCollapsed ? (
            <TbChevronRight size={22} />
          ) : (
            <TbChevronLeft size={22} />
          )}
        </ActionIcon>
      </div>

      {isCollapsed ? (
        <TradeElementCollapsed tradeElement={tradeElement}/>
      ) : (
        <TradeElementExpanded
          tradeElement={tradeElement}
          onElementContentUpdate={onElementContentUpdate}
          onElementAction={onElementAction}
        />
      )}
    </Paper>
  );
}

export default React.memo(TradeElementWrapper);
