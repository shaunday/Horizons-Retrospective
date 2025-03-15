import React from "react";
import { Badge, ThemeIcon } from "@mantine/core";
import { TRADE_STATUS } from "@constants/journalConstants";
import { getTradeStatusIcon } from "./tradeStatusIcons";

function TradeCompositeBadge({ tradeComposite }) {
    const StatusIcon = getTradeStatusIcon(tradeComposite[TRADE_STATUS]);

    return (
        <Badge size="lg" color="blue.0" p={0}>
            <ThemeIcon variant="light" >
                {StatusIcon && <StatusIcon size={18} />}
            </ThemeIcon>
        </Badge>
    );
}

export default React.memo(TradeCompositeBadge);