import React from "react";

const successMessageStyle = {
  color: "green",
  fontWeight: "bold",
  marginTop: "5px",
};

function SuccessMessage() {
  return <div style={successMessageStyle}>✔️ Success!</div>;
}

export default React.memo(SuccessMessage);