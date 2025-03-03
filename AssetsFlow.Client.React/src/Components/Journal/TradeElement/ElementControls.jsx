import React from "react";
import { Button } from '@mantine/core';
import ProcessingAndSuccessMessage from "@components/Processing/ProcessingAndSuccessMessage";
import { useProcessingWrapper } from "@hooks/useProcessingWrapper";
import { ProcessingStatus } from "@constants/Constants"; 

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

function ElementControls({ tradeElement }) {
  const { processingStatus, setNewStatus } = useProcessingWrapper();

  const handleAction = (action) => {
    setNewStatus(ProcessingStatus.SUCCESS)
    // tradeActionMutation.mutate(action);
  };

  return (
    <>
      <div style={buttonContainerStyle}>
        <Button
          size="xs"
          style={buttonStyle}
          onClick={handleAction}
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
