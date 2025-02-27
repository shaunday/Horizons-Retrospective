import React, { useState } from "react";
import SuccessMessage from "@components/SuccessMessage";
import { Button } from '@mantine/core';

const MemoizedSuccessMessage = React.memo(SuccessMessage);

// Extracted style for button container (center alignment for both axes)
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
  const [processing, setProcessing] = useState(false);
  const [success, setSuccess] = useState(false);

  const handleAction = (action) => {
    setProcessing(true);
    setSuccess(false);
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
      {processing && <div className="spinner">Processing...</div>}
      {success && <MemoizedSuccessMessage />}
    </>
  );
}

export default React.memo(ElementControls);
