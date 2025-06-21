import React from "react";
import { ActionIcon, Tooltip } from "@mantine/core";
import { TbPlus, TbMinus, TbMessageCircle, TbDoorExit } from "react-icons/tb";
import * as Constants from "@constants/journalConstants";

function ActionButtons({ tradeStatus, handleActionClick, disallowInteractions }) {
    // Generic ActionButton component
    const ActionButton = ({ tradeAction }) => {
        const variant = "subtle";
        const color = "gray";
        const className = "shadow-md hover:shadow-lg transition-all duration-200 hover:scale-110 bg-gray-100";
        
        const onButtonClick = () => {
            if (!disallowInteractions) {
                handleActionClick(tradeAction);
            }
        };
        
        const getActionConfig = (tradeAction) => {
            switch (tradeAction) {
                case Constants.TradeActions.ADD:
                    return {
                        icon: TbPlus,
                        label: "Add to position",
                        hoverClass: 'hover:bg-blue-100'
                    };
                case Constants.TradeActions.REDUCE:
                    return {
                        icon: TbMinus,
                        label: "Reduce position",
                        hoverClass: 'hover:bg-orange-100'
                    };
                case Constants.TradeActions.EVALUATE:
                    return {
                        icon: TbMessageCircle,
                        label: "Add Evaluation",
                        hoverClass: 'hover:bg-emerald-100'
                    };
                case Constants.TradeActions.CLOSE:
                    return {
                        icon: TbDoorExit,
                        label: "Close Trade",
                        hoverClass: 'hover:bg-red-100'
                    };
                default:
                    return {
                        icon: TbPlus,
                        label: "Action",
                        hoverClass: 'hover:bg-gray-200'
                    };
            }
        };
        
        const { icon: Icon, label, hoverClass } = getActionConfig(tradeAction);
        const fullClassName = `${className} ${hoverClass}`;
        
        return (
            <div style={{ pointerEvents: disallowInteractions ? 'none' : 'auto' }}>
                <Tooltip label={label} position="bottom" withArrow>
                    <ActionIcon
                        variant={variant}
                        color={color}
                        size="lg"
                        onClick={onButtonClick}
                        disabled={disallowInteractions}
                        className={fullClassName}
                    >
                        <Icon size={20} />
                    </ActionIcon>
                </Tooltip>
            </div>
        );
    };

    return (
        <div className="flex items-center space-x-2">
            <ActionButton
                tradeAction={Constants.TradeActions.ADD}
            />
            
            {tradeStatus === Constants.TradeStatus.OPEN && (
                <>
                    <ActionButton
                        tradeAction={Constants.TradeActions.REDUCE}
                    />
                    
                    <ActionButton
                        tradeAction={Constants.TradeActions.EVALUATE}
                    />
                    
                    <ActionButton
                        tradeAction={Constants.TradeActions.CLOSE}
                    />
                </>
            )}
        </div>
    );
}

export default ActionButtons;