import React from "react";
import { Button } from '@mantine/core';
import * as Constants from "@constants/journalConstants";
import { ElementActions } from "@constants/journalConstants";
import ProcessingAndSuccessMessage from "@components/Processing/ProcessingAndSuccessMessage";
import { useElementActionMutation } from "@hooks/Journal/Element/useElementActionMutation"
import { useRemoveElementFromTrade } from "@hooks/Journal/Element/useRemoveElementFromTrade"

const buttonContainerStyle = {
  display: "flex",
  justifyContent: "center", // Center horizontally
  alignItems: "center", // Center vertically
  marginLeft: "5px",
  height: "100%", // Ensure full height to center vertically
};

const buttonStyle = {
  marginRight: "5px"
};

function ElementControls({ tradeElement, onActionSuccess }) {
  const tradeId = tradeElement[Constants.ELEMENT_COMPOSITEFK_STING];
  const { removeElement } = useRemoveElementFromTrade(tradeId);

  const processTradeAction = ({action, response}) => {
    if (action === ElementActions.DELETE) {
      removeElement(tradeElement.id);
    }
    onActionSuccess(response);
  }
  const { elementActionMutation, processingStatus } = useElementActionMutation(tradeElement, processTradeAction);

  const handleAction = (action) => {
    elementActionMutation.mutate({ action });
  };

  return (
    <>
      <div style={buttonContainerStyle}>
        <Button
          size="xs"
          variant="outline"
          style={buttonStyle}
          onClick={() => handleAction(ElementActions.DELETE)}
        >
          Delete
          <br />
          Element
        </Button>
      </div>
      <ProcessingAndSuccessMessage status={processingStatus} />
    </>
  );
}

export default React.memo(ElementControls);
