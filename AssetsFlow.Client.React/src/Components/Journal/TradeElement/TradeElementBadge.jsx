import React from "react";
import { Badge } from "@mantine/core";
import * as Constants from "@constants/journalConstants";

const badgeStyle = {
    position: "absolute",
    top: "-10px",
    left: "20px",
};

function TradeElementBadge({ tradeElement }) {
    let timestamp = null;
    if (tradeElement[Constants.ELEMENT_TIMESTAMP_STING]) {
        timestamp = new Date(tradeElement[Constants.ELEMENT_TIMESTAMP_STING]).toLocaleString();
    }
    const type = tradeElement[Constants.ELEMENT_TYPE_STING];
    return (
        <Badge size="sm" color="blue.4" style={badgeStyle}>
            {type}{timestamp ? `. ${timestamp}` : ""}
        </Badge>
    );
}

export default React.memo(TradeElementBadge);