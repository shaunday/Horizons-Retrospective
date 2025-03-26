import React from "react";
import { Button } from "@mantine/core";
import * as Constants from "@constants/journalConstants";

function ActionButtons({ tradeStatus, handleActionClick, disallowInteractions }) {
    return (
        <div style={{ marginTop: "10px", marginBottom: "5px" }} className="element-to-be-centered">
            <Button
                mr={5}
                onClick={() => handleActionClick(Constants.TradeActions.ADD)}
                disabled={disallowInteractions}
            >
                Add to position
            </Button>
            {tradeStatus === Constants.TradeStatus.OPEN && (
                <>
                    <Button
                        mr={5}
                        onClick={() => handleActionClick(Constants.TradeActions.REDUCE)}
                        disabled={disallowInteractions}
                    >
                        Reduce position
                    </Button>
                    <Button
                        mr={5}
                        onClick={() => handleActionClick(Constants.TradeActions.EVALUATE)}
                        disabled={disallowInteractions}
                    >
                        Add Evaluation
                    </Button>
                    <Button
                        mr={5}
                        onClick={() => handleActionClick(Constants.TradeActions.CLOSE)}
                        disabled={disallowInteractions}
                    >
                        Close Trade
                    </Button>
                </>
            )}
        </div>
    );
}

export default ActionButtons;