import React, { useState } from "react";
import SuccessMessage from "@components/SuccessMessage";

const MemoizedSuccessMessage = React.memo(SuccessMessage);

// Extracted style for button container (center alignment for both axes)
const buttonContainerStyle = {
  display: "flex",
  justifyContent: "center", // Center horizontally
  alignItems: "center", // Center vertically
  marginRight: "10px",
  height: "100%", // Ensure full height to center vertically
};

const buttonStyle = {
  display: "inline-block",
  marginLeft:"5px"
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
        <button
          type="button"
          style={buttonStyle}
          onClick={handleAction}
        >
          X
        </button>
      </div>
      {processing && <div className="spinner">Processing...</div>}
      {success && <MemoizedSuccessMessage />}
    </>
  );
}

export default React.memo(ElementControls);
