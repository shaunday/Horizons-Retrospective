import { createContext, useState } from "react";

const [value, setValue] = useState(undefined);

ReferenceDataContext = createContext({ value, setValue });

const ReferenceDataContextProvider = ({ children }) => {
    return (
      <ReferenceDataContext.Provider value={{ value, setValue }}>
        {...children}
      </ReferenceDataContext.Provider>
    );
  };
  
  export { ReferenceDataContext, ReferenceDataContextProvider };