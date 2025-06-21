import React from "react";
import { Badge, ThemeIcon, Tooltip } from "@mantine/core";
import { TbProgressAlert } from "react-icons/tb";
import * as Constants from "@constants/journalConstants";

function TradeNotifications({ tradeComposite }) {
  if (tradeComposite[Constants.TRADE_ISPENDING]) {
    return (
      <Tooltip label="Some required fields are empty or invalid." withArrow>
        <Badge
          size="sm"
          color="red.3"
          variant="light"
          radius="md"
          leftSection={<TbProgressAlert size={14} />}
          className="hover:bg-red-400 hover:text-white transition-colors"
        >
          Missing
        </Badge>
      </Tooltip>
    );
  }

  return null;
}

export default React.memo(TradeNotifications);
