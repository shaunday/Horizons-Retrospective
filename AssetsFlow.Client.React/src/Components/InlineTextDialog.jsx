import React, { useState } from "react";

const styles = {
  container: {
    display: "flex",
    alignItems: "center",
    cursor: "pointer",
    verticalAlign: "middle", 
  },
  editableBox: {
    padding: "5px",
    border: "1px solid #ccc",
    borderRadius: "4px",
  }
};

function InlineTextDialog({ valueForDisplay, onApplyChanges }) {
  const [displayValue, setDisplayValue] = useState(valueForDisplay);

  const handleClickOnInput = () => {
    const userInput = window.prompt("Edit Value:", displayValue);
    if (userInput !== null) { // null means the user canceled
      onApplyChanges(userInput);
    }
  };

  return (
    <div style={styles.container}>
      <div
        onClick={handleClickOnInput}
        style={styles.editableBox}
      >
        {valueForDisplay || "Click to edit"} {/* Show placeholder if empty */}
      </div>
    </div>
  );
}

export default React.memo(InlineTextDialog);
