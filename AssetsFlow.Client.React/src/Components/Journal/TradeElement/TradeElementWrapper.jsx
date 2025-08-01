import React, { useState } from "react";
import { ActionIcon, Paper } from "@mantine/core";
import { TbChevronRight, TbChevronLeft } from "react-icons/tb";
import clsx from "clsx";
import TradeElementExpanded from "./TradeElementExpanded";
import TradeElementCollapsed from "./TradeElementCollapsed";
import TradeElementBadge from "./Badge/TradeElementBadge";

function TradeElementWrapper({ tradeElement }) {
  const [isCollapsed, setIsCollapsed] = useState(true);

  return (
    <Paper className="py-1 pr-2.5 pl-6 h-full relative flex items-center border border-slate-200 bg-slate-50 w-fit">
      <div className="absolute -left-3 top-1/2 rounded-md -translate-y-1/2">
        <TradeElementBadge tradeElement={tradeElement} />
      </div>

      <div className="mr-2">
        <ActionIcon
          variant="subtle"
          onClick={() => setIsCollapsed((prev) => !prev)}
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
        <div className="pl-3">
          <TradeElementExpanded tradeElement={tradeElement} />
        </div>
      )}
    </Paper>
  );
}

export default React.memo(TradeElementWrapper);
