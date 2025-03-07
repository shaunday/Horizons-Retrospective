import React, { useState } from "react";
import * as Constants from "@constants/journalConstants";
import {ProcessingStatus} from "@constants/constants";
import ProcessingAndSuccessMessage from "@components/Processing/ProcessingAndSuccessMessage";
import ActionButtons from "./ActionButtons";
import TradeClosingModal from "./TradeClosingModal";
import { useTradeActionMutation } from "@hooks/Journal/Composite/useTradeActionMutation";
import { useProcessingWrapper } from "@hooks/useProcessingWrapper";

function CompositeControls({ tradeComposite, onTradeActionExecuted }) {
  const { processingStatus, setNewStatus } = useProcessingWrapper(ProcessingStatus.NONE); 
  const [isModalOpen, setIsModalOpen] = useState(false);

  const tradeActionMutation = useTradeActionMutation(
    tradeComposite,
    (newElement, newSummary) => {
      onTradeActionExecuted(newElement, newSummary);
      setNewStatus(ProcessingStatus.SUCCESS); 
    }
  );

  const handleAction = (action) => {
    if (action === Constants.TradeActions.CLOSE) {
      setIsModalOpen(true);
    } else {
      setNewStatus(ProcessingStatus.PROCESSING);
      tradeActionMutation.mutate(action, null);
    }
  };

  const handleCloseTrade = (closingPrice) => {
    setNewStatus(ProcessingStatus.PROCESSING); 
    tradeActionMutation.mutate(Constants.TradeActions.CLOSE, closingPrice);
    setIsModalOpen(false);
  };

  return (
    <div>
      <ActionButtons
        tradeStatus={tradeComposite[Constants.TRADE_STATUS_STRING]}
        handleActionClick={handleAction}
      />
      <ProcessingAndSuccessMessage status={processingStatus} />

      <TradeClosingModal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        onSubmit={handleCloseTrade}
      />
    </div>
  );
}

export default React.memo(CompositeControls);