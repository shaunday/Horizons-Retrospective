import React, { useState } from "react";
import { ActionIcon, Paper } from "@mantine/core";
import { TbChevronDownRight, TbChevronUpLeft } from "react-icons/tb";
import clsx from "clsx";
import TradeExpanded from "./TradeExpanded";
import TradeCollapsed from "./TradeCollapsed";
import { useGetTradeById } from "@hooks/Journal/useGetTradeById";
import TradeCompositeBadge from "./Badge/TradeCompositeBadge";

function TradeWrapper({ tradeId, isNewTrade = false }) {
  const { trade } = useGetTradeById(tradeId);
  const [isCollapsed, setIsCollapsed] = useState(!isNewTrade);

  if (!trade) {
    return <div>Loading...</div>;
  }

  return (
    <Paper
      shadow="md"
      withBorder
      className={clsx("h-full p-1 mb-1 flex items-center justify-start", {
        "bg-slate-100 border-slate-400": !isCollapsed,
        "bg-slate-50": isCollapsed
      })}
    >
      <div className={clsx("flex gap-1", {
        "flex-row": isCollapsed,
        "flex-col": !isCollapsed,
        "mr-2.5": isCollapsed,
        "mr-4": !isCollapsed
      })}>
        <ActionIcon
          variant="subtle"
          onClick={() => setIsCollapsed((prev) => !prev)}
          className={clsx({
            "h-12": !isCollapsed
          })}
        >
          {isCollapsed ? (
            <TbChevronDownRight size={22} />
          ) : (
            <TbChevronUpLeft size={22} className="h-12" />
          )}
        </ActionIcon>
        {isCollapsed && <TradeCompositeBadge tradeComposite={trade} />}
      </div>

      <div className="flex-1">
        {isCollapsed ? <TradeCollapsed trade={trade} /> : <TradeExpanded trade={trade} />}
      </div>

    </Paper>
  );
}

export default React.memo(TradeWrapper);
