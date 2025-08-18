import React from "react";
import { ProcessingStatus } from "@constants/Constants"; 

// SuccessMessage component
function SuccessMessage() {
  return <div className="mt-1 text-green-600 font-bold">✔️ Success!</div>;
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