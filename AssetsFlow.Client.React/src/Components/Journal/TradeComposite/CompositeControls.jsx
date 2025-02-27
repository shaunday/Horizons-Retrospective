import React, { useState } from "react";
import * as Constants from "@constants/journalConstants";
import SuccessMessage from "@components/SuccessMessage";
import { useTradeActionMutation } from "@hooks/Composite/useTradeActionMutation";
import { Button } from "@mantine/core";

const MemoizedSuccessMessage = React.memo(SuccessMessage);

const buttonStyles = { marginRight: "5px" };
const containerStyle = { marginTop: "5px", marginRight: "auto", marginLeft: "auto"};

function CompositeControls({ tradeComposite, onTradeActionExecuted }) {
  const [processing, setProcessing] = useState(false);
  const [success, setSuccess] = useState(false);

  const tradeActionMutation = useTradeActionMutation(
    tradeComposite,
    (newElement, newSummary) => {
      onTradeActionExecuted(newElement, newSummary);
      setProcessing(false);
      setSuccess(true);
      setTimeout(() => setSuccess(false), 2000);
    }
  );

  const handleAction = (action) => {
    setProcessing(true);
    setSuccess(false);
    tradeActionMutation.mutate(action);
  };

  const handleActionClick = (action) => () => handleAction(action);

  return (
    <>
      <div style={containerStyle}>
        <Button
          style={buttonStyles}
          onClick={handleActionClick(Constants.TradeActions.ADD)}
        >
          Add to position
        </Button>
        {tradeComposite[Constants.TRADE_STATUS_STRING] === Constants.TradeStatus.OPEN && (
          <Button
            style={buttonStyles}
            onClick={handleActionClick(Constants.TradeActions.REDUCE)}
          >
            Reduce position
          </Button>
        )}
        {tradeComposite[Constants.TRADE_STATUS_STRING] === Constants.TradeStatus.OPEN && (
          <Button
            style={buttonStyles}
            onClick={handleActionClick(Constants.TradeActions.EVALUATE)}
          >
            Add Evaluation
          </Button>
        )}
        {tradeComposite[Constants.TRADE_STATUS_STRING] === Constants.TradeStatus.OPEN && (
          <Button
            style={buttonStyles}
            onClick={handleActionClick(Constants.TradeActions.CLOSE)}
          >
            Close Trade
          </Button>
        )}
      </div>
      {processing && <div className="spinner">Processing...</div>}
      {success && <MemoizedSuccessMessage />}
    </>
  );
}

export default React.memo(CompositeControls);
