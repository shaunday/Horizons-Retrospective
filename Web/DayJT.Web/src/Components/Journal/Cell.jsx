import React, { useState } from "react";
import { useContentUpdateMutation } from "@hooks/useContentUpdateMutation";
import SuccessMessage from "@components/SuccessMessage";

const MemoizedSuccessMessage = React.memo(SuccessMessage);

function Cell({ cellInfo, onCellUpdate }) {
  const [displayValue, setDisplayValue] = useState(
    cellInfo.ContentWrapper.Content
  );

  const { contentUpdateMutation, processing, success } =
    useContentUpdateMutation(cellInfo, onCellUpdate);

  const handleKeyPress = (e) => {
    if (e.key === "Enter") {
      contentUpdateMutation.mutate(displayValue);
    }
  };

  return (
    <>
      <div id="cellTitle">{cellInfo.ContentWrapper.Title}</div>
      <input
        id="cellInput"
        type="text"
        value={displayValue}
        onChange={(e) => setDisplayValue(e.target.value)}
        onKeyDown={handleKeyPress}
        disabled={processing}
        style={success ? { borderColor: "green" } : {}}
      />
      {processing && <div className="spinner">Processing...</div>}
      {success && <MemoizedSuccessMessage />}
    </>
  );
}

export default React.memo(Cell);
