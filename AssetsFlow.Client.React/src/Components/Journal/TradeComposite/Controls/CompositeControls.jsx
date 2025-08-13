import React, { useState } from "react";
import * as Constants from "@constants/journalConstants";
import ProcessingAndSuccessMessage from "@components/Processing/ProcessingAndSuccessMessage";
import ActionButtons from "./ActionButtons";
import TradeClosingModal from "./TradeClosingModal";
import PropTypes from "prop-types";
import { useTradeActionMutation } from "@hooks/Journal/Composite/useTradeActionMutation";

function CompositeControls({ tradeComposite }) {
  const [isModalOpen, setIsModalOpen] = useState(false);
  const { tradeActionMutation, processingStatus } = useTradeActionMutation(tradeComposite);

  const handleAction = (action) => {
    if (action === Constants.TradeActions.CLOSE) {
      setIsModalOpen(true);
    } else {
      tradeActionMutation.mutate({ action, additionalParam: null });
    }
  };

  const handleCloseTrade = (closingPrice) => {
    tradeActionMutation.mutate({
      action: Constants.TradeActions.CLOSE,
      additionalParam: closingPrice,
    });
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

CompositeControls.propTypes = {
  tradeComposite: PropTypes.object.isRequired,
};

export default React.memo(CompositeControls);
