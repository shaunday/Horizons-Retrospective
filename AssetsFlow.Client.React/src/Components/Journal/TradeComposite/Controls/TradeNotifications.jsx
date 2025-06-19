import React from "react";
import { Badge, ThemeIcon, Tooltip } from "@mantine/core";
import { TbProgressAlert } from "react-icons/tb";
import * as Constants from "@constants/journalConstants";

function TradeNotifications({ tradeComposite }) {
  if (tradeComposite[Constants.TRADE_ISPENDING]) {
    return (
      <Tooltip label="Some content is missing..." withArrow>
        <Badge
          size="sm"
          color="red"
          variant="light"
          radius="md"
          leftSection={<TbProgressAlert size={14} />}
        >
          Missing
        </Badge>
      </Tooltip>
    );
  }

  return null;
}

export default React.memo(TradeNotifications);
