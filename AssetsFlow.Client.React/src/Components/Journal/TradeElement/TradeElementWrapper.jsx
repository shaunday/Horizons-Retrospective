import React, { useState } from "react";
import { ActionIcon, Paper } from "@mantine/core";
import { TbChevronRight, TbChevronLeft } from "react-icons/tb";
import clsx from "clsx";
import TradeElementExpanded from "./TradeElementExpanded";
import TradeElementCollapsed from "./TradeElementCollapsed";
import TradeElementBadge from "./Badge/TradeElementBadge";

function TradeElementWrapper({ tradeElement, onElementContentUpdate, onElementAction }) {
  const [isCollapsed, setIsCollapsed] = useState(true);

  return (
    <Paper className="py-1 pr-2.5 pl-6 h-full relative flex items-center border border-slate-200 bg-slate-50 hover:bg-slate-75 hover:border-slate-300 hover:shadow-sm">
      <div className="absolute -left-3 top-1/2 rounded-md -translate-y-1/2">
        <TradeElementBadge tradeElement={tradeElement} />
      </div>

      <div className={clsx("mr-1 flex gap-1", {
        "flex-row": isCollapsed,
        "flex-col": !isCollapsed
      })}>
        <ActionIcon
          variant="subtle"
          onClick={() => setIsCollapsed((prev) => !prev)}
          className={clsx("hover:bg-slate-200", {
            "mr-2 h-12": !isCollapsed
          })}
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
