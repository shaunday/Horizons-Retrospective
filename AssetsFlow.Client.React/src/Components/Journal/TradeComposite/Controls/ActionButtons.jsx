import React from "react";
import { Button } from "@mantine/core";
import * as Constants from "@constants/journalConstants";

function ActionButtons ({ tradeStatus, handleActionClick }) {
    return (
        <div style={{ marginTop: "5px", marginRight: "auto", marginLeft: "auto" }}>
            <Button mr={5} onClick={() => handleActionClick(Constants.TradeActions.ADD)}>
                Add to position
            </Button>
            {tradeStatus === Constants.TradeStatus.OPEN && (
                <>
                    <Button mr={5} onClick={() => handleActionClick(Constants.TradeActions.REDUCE)}>
                        Reduce position
                    </Button>
                    <Button mr={5} onClick={() => handleActionClick(Constants.TradeActions.EVALUATE)}>
                        Add Evaluation
                    </Button>
                    <Button mr={5} onClick={() => handleActionClick(Constants.TradeActions.CLOSE)}>
                        Close Trade
                    </Button>
                </>
            )}
        </div>
    );
};

export default ActionButtons;