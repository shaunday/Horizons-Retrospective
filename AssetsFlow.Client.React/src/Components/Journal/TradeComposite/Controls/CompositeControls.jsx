import React, { useState } from "react";
import * as Constants from "@constants/journalConstants";
import { ProcessingStatus } from "@constants/constants";
import ProcessingAndSuccessMessage from "@components/Processing/ProcessingAndSuccessMessage";
import ActionButtons from "./ActionButtons";
import TradeClosingModal from "./TradeClosingModal";
import { useTradeActionMutation } from "@hooks/Journal/Composite/useTradeActionMutation";
import { useProcessingWrapper } from "@hooks/useProcessingWrapper";

function CompositeControls({ tradeComposite, onTradeActionSuccess }) {
  const { processingStatus, setNewStatus } = useProcessingWrapper(ProcessingStatus.NONE);
  const [isModalOpen, setIsModalOpen] = useState(false);

  const tradeActionMutation = useTradeActionMutation(
    tradeComposite,
    (response) => {
      onTradeActionSuccess(response);
      setNewStatus(ProcessingStatus.SUCCESS);
    }
  );

  const handleAction = (action) => {
    if (action === Constants.TradeActions.CLOSE) {
      setIsModalOpen(true);
    } else {
      setNewStatus(ProcessingStatus.PROCESSING);
      tradeActionMutation.mutate({ action, additionalParam: null });
    }
  };

  const handleCloseTrade = (closingPrice) => {
    setNewStatus(ProcessingStatus.PROCESSING);
    tradeActionMutation.mutate({ action: Constants.TradeActions.CLOSE, additionalParam: closingPrice });
    setIsModalOpen(false);
  };

  return (
    <div className="flex flex-col items-end gap-2">
      <ActionButtons
        tradeStatus={tradeComposite?.[Constants.TRADE_STATUS]}
        disallowInteractions={tradeComposite?.[Constants.HasMissingContent]}
        handleActionClick={handleAction}
      />
      
      <ProcessingAndSuccessMessage status={processingStatus} />

      <TradeClosingModal
        opened={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        onSubmit={handleCloseTrade}
      />
    </div>
  );
}

export default React.memo(CompositeControls);