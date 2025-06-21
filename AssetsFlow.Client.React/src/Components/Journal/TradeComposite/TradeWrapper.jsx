import React, { useState } from "react";
import { ActionIcon, Paper } from "@mantine/core";
import { TbChevronDownRight, TbChevronUpLeft } from "react-icons/tb";
import clsx from "clsx";
import TradeExpanded from "./TradeExpanded";
import TradeCollapsed from "./TradeCollapsed";
import { useGetTradeById } from "@hooks/Journal/useGetTradeById";
import TradeCompositeBadge from "./Badge/TradeCompositeBadge";
import TradeAdminControls from "./Controls/TradeAdminControls";

function TradeWrapper({ tradeId, indexType }) {
  const { trade } = useGetTradeById(tradeId);
  const [isCollapsed, setIsCollapsed] = useState(true);

  const handleClick = (e) => {
    // Check if the clicked element or its parent has data-no-toggle
    const hasNoToggle = e.target.closest('[data-no-toggle]');
    if (hasNoToggle) {
      return; // Don't toggle if clicking on an element with data-no-toggle
    }
    setIsCollapsed((prev) => !prev);
  };

  if (!trade) {
    return <div>Loading...</div>;
  }

  return (
    <Paper
      shadow="md"
      withBorder
      className={clsx("h-full p-1 mb-1 flex items-center justify-start cursor-pointer", {
        "bg-slate-100 border-slate-400": !isCollapsed,
        "bg-slate-50": isCollapsed
      })}
      onClick={handleClick}
    >
      {/* Interactive elements that block background clicks */}
      <div className={clsx("flex gap-1 pointer-events-auto", {
        "flex-row": isCollapsed,
        "flex-col": !isCollapsed,
        "mr-2.5": isCollapsed,
        "mr-4": !isCollapsed
      })}>
        <ActionIcon
          variant="subtle"
          data-no-toggle
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

      {/* Trade content that blocks background clicks */}
      <div data-no-toggle className="flex-1">
        {isCollapsed ? <TradeCollapsed trade={trade} /> : <TradeExpanded trade={trade} />}
      </div>

    </Paper>
  );
}

export default React.memo(TradeWrapper);
