import React from "react";
import PropTypes from "prop-types";
import { ProcessingStatus } from "@constants/constants";

function SuccessMessage() {
  return <div className="mt-1 text-green-600 font-bold">✔️ Success!</div>;
}

const MemoizedSuccessMessage = React.memo(SuccessMessage);

function ProcessingAndSuccessMessage({ status }) {
  if (!Object.values(ProcessingStatus).includes(status)) {
    console.warn("ProcessingAndSuccessMessage: Invalid status prop", status);
    return null;
  }

  return (
    <>
      {status === ProcessingStatus.PROCESSING && <div className="spinner">Processing...</div>}
      {status === ProcessingStatus.SUCCESS && <MemoizedSuccessMessage />}
    </>
  );
}

ProcessingAndSuccessMessage.propTypes = {
  status: PropTypes.oneOf(Object.values(ProcessingStatus)).isRequired,
};

export default React.memo(ProcessingAndSuccessMessage);
