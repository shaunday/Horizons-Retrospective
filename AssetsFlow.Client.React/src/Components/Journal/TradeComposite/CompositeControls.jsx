import React, { useState } from "react";
import * as Constants from "@constants/journalConstants";
import SuccessMessage from "@components/SuccessMessage";
import { useTradeAction } from "@hooks/Composite/useTradeAction"; // Import custom hook

const MemoizedSuccessMessage = React.memo(SuccessMessage);

function CompositeControls({ tradeComposite, onTradeActionExecuted }) {
  const [processing, setProcessing] = useState(false);
  const [success, setSuccess] = useState(false);

  const tradeActionMutation = useTradeAction(
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
      <div style={{ textAlign: "right", marginRight: "10px" }}>
        <button
          className="button-38"
          type="button"
          style={{ display: "inline-block", marginRight: "10px" }}
          onClick={handleActionClick(Constants.TradeActions.ADD)}
        >
          Add to position
        </button>
        {tradeComposite[Constants.TRADE_STATUS_STRING] == Constants.TradeStatus.OPEN && <button
          className="button-38"
          type="button"
          style={{ display: "inline-block" }}
          onClick={handleActionClick(Constants.TradeActions.REDUCE)}
        >
          Reduce position
        </button>}
        {tradeComposite[Constants.TRADE_STATUS_STRING] == Constants.TradeStatus.OPEN && <button
          className="button-38"
          type="button"
          style={{ display: "inline-block" }}
          onClick={handleActionClick(Constants.TradeActions.CLOSE)}
        >
          Close Trade
        </button>}
      </div>
      {processing && <div className="spinner">Processing...</div>}
      {success && <MemoizedSuccessMessage />}
    </>
  );
}

export default React.memo(CompositeControls);
