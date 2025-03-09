import React from "react";
import { Badge } from "@mantine/core";
import * as Constants from "@constants/journalConstants";

const styles = {
    badge: {
        position: "absolute",
        top: "-10px",
        left: "20px",
        zIndex: 10,
    },
};

function TradeElementBadge({ tradeElement }) {
    const timestamp = tradeElement[Constants.ELEMENT_TIMESTAMP_STING];
    const type = tradeElement[Constants.ELEMENT_TYPE_STING];

    return (
        <Badge size="sm" color="blue.4" style={styles.badge}>
            {type}{timestamp ? `. ${timestamp}` : ""}
        </Badge>
    );
}

export default React.memo(TradeElementBadge);