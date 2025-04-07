import React, { useState } from "react";
import { ActionIcon, Stack } from "@mantine/core";
import { TbChevronRight, TbChevronLeft } from "react-icons/tb";
import TradeElementExpanded from "./TradeElementExpanded"
import TradeElementCollapsed from "./TradeElementCollapsed"

const styles = {
    elementItem: {
        border: "1.5px solid blue",
        padding: "5px",
        borderRadius: "4px",
        marginBottom: "5px",
        display: "flex",
        alignItems: "center",
        height: "100%",
    },
};

function TradeElementWrapper({ tradeElement, onElementContentUpdate, onElementAction }) {
    const [isCollapsed, setIsCollapsed] = useState(true);

    return (
        <div style={styles.elementItem}>
            <Stack gap="xs" style={{ margin: "0 7px 0 1px", alignItems: "center" }}>
                <ActionIcon
                    variant="subtle"
                    onClick={() => setIsCollapsed((prev) => !prev)}
                    style={{ ...(isCollapsed ? {} : { height: 50 }) }}
                >
                    {isCollapsed ? <TbChevronRight size={22} /> : <TbChevronLeft size={22} style={{ height: "50px" }} />}
                </ActionIcon>
            </Stack>
            {isCollapsed ? <TradeElementCollapsed tradeElement={tradeElement}  isTradeOverview={false} /> :
                <TradeElementExpanded tradeElement={tradeElement} onElementContentUpdate={onElementContentUpdate} onElementAction={onElementAction} />}
        </div>
    );
}

export default React.memo(TradeElementWrapper);