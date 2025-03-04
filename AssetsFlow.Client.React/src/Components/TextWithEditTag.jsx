import React from 'react';

const editableBoxStyle = {
    padding: "5px",
    border: "1px dotted #ccc",
};

function TextWithEditTag({ valueForDisplay, onEditRequested }) {

  return (
    <div className="centered-text editable-text"
        onClick={onEditRequested}
        style={editableBoxStyle}
      >
       {valueForDisplay}
      </div>
  );
}

export default React.memo(TextWithEditTag);