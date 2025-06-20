import React from "react";
import { Button } from "@mantine/core";
import * as Constants from "@constants/journalConstants";

function ActionButtons({ tradeStatus, handleActionClick, disallowInteractions }) {
    return (
        <div className="mt-2.5 mb-1.5 element-to-be-centered">
            <Button
                className="mr-1"
                onClick={() => handleActionClick(Constants.TradeActions.ADD)}
                disabled={disallowInteractions}
            >
                Add to position
            </Button>
            {tradeStatus === Constants.TradeStatus.OPEN && (
                <>
                    <Button
                        className="mr-1"
                        onClick={() => handleActionClick(Constants.TradeActions.REDUCE)}
                        disabled={disallowInteractions}
                    >
                        Reduce position
                    </Button>
                    <Button
                        className="mr-1"
                        onClick={() => handleActionClick(Constants.TradeActions.EVALUATE)}
                        disabled={disallowInteractions}
                    >
                        Add Evaluation
                    </Button>
                    <Button
                        className="mr-1"
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