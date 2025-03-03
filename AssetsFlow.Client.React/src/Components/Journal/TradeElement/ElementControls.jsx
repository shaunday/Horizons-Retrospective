import React from "react";
import { Button } from '@mantine/core';
import { ElementActions } from "@constants/journalConstants";
import ProcessingAndSuccessMessage from "@components/Processing/ProcessingAndSuccessMessage";
import { useElementActionMutation } from "@hooks/Journal/Element/useElementActionMutation"

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
  const { elementActionMutation, processingStatus } = useElementActionMutation(tradeElement, onActionSuccess);

  const handleAction = (action) => {
    elementActionMutation.mutate({action});
  };

  return (
    <>
      <div style={buttonContainerStyle}>
        <Button
          size="xs"
          variant="outline"
          style={buttonStyle}
          onClick={()=> handleAction(ElementActions.DELETE)}
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
