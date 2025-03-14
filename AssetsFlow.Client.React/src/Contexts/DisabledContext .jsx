import React, { createContext, useContext, useState } from "react";

const DisabledContext = createContext(false);
export const useDisabled = () => useContext(DisabledContext);

export const DisabledProvider = ({ children }) => {
  const [disabled, setDisabled] = useState(false);
  const enableControls = () => setDisabled(false);
  const disableControls = () => setDisabled(true);

  return (
    <DisabledContext.Provider value={{ disabled, enableControls, disableControls }}>
      {children}
    </DisabledContext.Provider>
  );
};
