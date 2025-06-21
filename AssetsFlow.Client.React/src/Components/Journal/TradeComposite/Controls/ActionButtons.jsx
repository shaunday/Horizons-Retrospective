import React from "react";
import { ActionIcon, Tooltip } from "@mantine/core";
import { TbPlus, TbMinus, TbMessageCircle, TbDoorExit } from "react-icons/tb";
import * as Constants from "@constants/journalConstants";

function ActionButtons({ tradeStatus, handleActionClick, disallowInteractions }) {
    return (
        <div className="flex items-center space-x-2">
            <Tooltip label="Add to position" position="bottom" withArrow>
                <ActionIcon
                    variant="filled"
                    color="blue"
                    size="lg"
                    onClick={() => handleActionClick(Constants.TradeActions.ADD)}
                    disabled={disallowInteractions}
                    className="shadow-md hover:shadow-lg transition-all duration-200 hover:scale-110"
                >
                    <TbPlus size={20} />
                </ActionIcon>
            </Tooltip>
            
            {tradeStatus === Constants.TradeStatus.OPEN && (
                <>
                    <Tooltip label="Reduce position" position="bottom" withArrow>
                        <ActionIcon
                            variant="filled"
                            color="orange"
                            size="lg"
                            onClick={() => handleActionClick(Constants.TradeActions.REDUCE)}
                            disabled={disallowInteractions}
                            className="shadow-md hover:shadow-lg transition-all duration-200 hover:scale-110"
                        >
                            <TbMinus size={20} />
                        </ActionIcon>
                    </Tooltip>
                    
                    <Tooltip label="Add Evaluation" position="bottom" withArrow>
                        <ActionIcon
                            variant="filled"
                            color="green"
                            size="lg"
                            onClick={() => handleActionClick(Constants.TradeActions.EVALUATE)}
                            disabled={disallowInteractions}
                            className="shadow-md hover:shadow-lg transition-all duration-200 hover:scale-110"
                        >
                            <TbMessageCircle size={20} />
                        </ActionIcon>
                    </Tooltip>
                    
                    <Tooltip label="Close Trade" position="bottom" withArrow>
                        <ActionIcon
                            variant="filled"
                            color="red"
                            size="lg"
                            onClick={() => handleActionClick(Constants.TradeActions.CLOSE)}
                            disabled={disallowInteractions}
                            className="shadow-md hover:shadow-lg transition-all duration-200 hover:scale-110"
                        >
                            <TbDoorExit size={20} />
                        </ActionIcon>
                    </Tooltip>
                </>
            )}
        </div>
    );
}

export default ActionButtons;