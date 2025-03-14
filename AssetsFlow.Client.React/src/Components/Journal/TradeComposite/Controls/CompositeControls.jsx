import React, { useState } from "react";
import * as Constants from "@constants/journalConstants";
import { Group } from "@mantine/core";
import { ProcessingStatus } from "@constants/constants";
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
      tradeActionMutation.mutate({ action, additionalParam: null });
    }
  };

  const handleCloseTrade = (closingPrice) => {
    setNewStatus(ProcessingStatus.PROCESSING);
    tradeActionMutation.mutate({ action: Constants.TradeActions.CLOSE, additionalParam: closingPrice });
    setIsModalOpen(false);
  };

  return (
    <Group>
      <ActionButtons
        tradeStatus={tradeComposite?.[Constants.TRADE_STATUS]}
        handleActionClick={handleAction}
      />
      <ProcessingAndSuccessMessage status={processingStatus} />

      <div style={{ overflow: 'visible', height: '100%' }}>
        <TradeClosingModal
          opened={isModalOpen}
          onClose={() => setIsModalOpen(false)}
          onSubmit={handleCloseTrade}
        />
      </div>
    </Group>
  );
}

export default React.memo(CompositeControls);