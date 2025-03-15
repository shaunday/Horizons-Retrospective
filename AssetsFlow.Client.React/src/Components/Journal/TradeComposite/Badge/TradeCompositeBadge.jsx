import React from "react";
import { Badge, ThemeIcon, Tooltip } from "@mantine/core";
import { TRADE_STATUS } from "@constants/journalConstants";
import { getTradeStatusIcon } from "./tradeStatusIcons";

function TradeCompositeBadge({ tradeComposite }) {
    const StatusIcon = getTradeStatusIcon(tradeComposite[TRADE_STATUS]);
    const statusText = tradeComposite?.[TRADE_STATUS] || "Unknown Status";  // Get status text

    return (
        <Tooltip label={statusText} withArrow>
            <Badge size="lg" color="grey.7" p={0}>
                <ThemeIcon variant="light">
                    {StatusIcon && <StatusIcon size={18} />}
                </ThemeIcon>
            </Badge>
        </Tooltip>
    );
}

export default React.memo(TradeCompositeBadge);