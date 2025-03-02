import React from 'react';

const styles = {
  container: {
    display: "flex",
    alignItems: "center",
    cursor: "pointer",
    verticalAlign: "middle",
    width: "100%",
  },
  editableBox: {
    padding: "5px",
    border: "1px solid #ccc",
    borderRadius: "4px",
    width: "100%",
  }
};

function TextWithEditTag({ valueForDisplay, onEditRequested }) {

  return (
    <div style={styles.container}>
      <div
        onClick={onEditRequested}
        style={styles.editableBox}
      >
        {valueForDisplay || "Click to edit"} {/* Show placeholder if empty */}
      </div>
    </div>
  );
}

export default React.memo(TextWithEditTag);