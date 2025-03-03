import React from "react";
import { ProcessingStatus } from "@constants/Constants"; 

// SuccessMessage component
const successMessageStyle = {
  color: "green",
  fontWeight: "bold",
  marginTop: "5px",
};

function SuccessMessage() {
  return <div style={successMessageStyle}>✔️ Success!</div>;
}

const MemoizedSuccessMessage = React.memo(SuccessMessage);

function ProcessingAndSuccessMessage ({ status }) {
  return (
    <>
      {status === ProcessingStatus.PROCESSING && (
        <div className="spinner">Processing...</div>
      )}
      {status === ProcessingStatus.SUCCESS && <MemoizedSuccessMessage />}
    </>
  );
};

export default React.memo(ProcessingAndSuccessMessage);