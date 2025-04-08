import React from "react";
import { Badge, Tooltip, ThemeIcon } from "@mantine/core";
import * as Constants from "@constants/journalConstants";
import { getTradeElementStatusIcon } from "./tradeElementIcons";

function TradeElementBadge({ tradeElement }) {
    const elementType = tradeElement[Constants.ELEMENT_TYPE_STING];
    const StatusIcon = getTradeElementStatusIcon(elementType);

    let timestamp = null;
    if (tradeElement[Constants.ELEMENT_TIMESTAMP_STING]) {
        timestamp = new Date(tradeElement[Constants.ELEMENT_TIMESTAMP_STING]).toLocaleString();
    }


    const statusText = elementType + timestamp ? `. ${timestamp}` : ""
    return (
        <Tooltip label={statusText} withArrow>
            <Badge size="lg" color="grey.7" p={0}>
                <ThemeIcon variant="white">
                    {StatusIcon && <StatusIcon size={18} />}
                </ThemeIcon>
            </Badge>
        </Tooltip>
    );
}

export default React.memo(TradeElementBadge);