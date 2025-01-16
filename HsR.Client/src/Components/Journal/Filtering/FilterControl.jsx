import React, { useState } from "react";
import LabeledField from "@components/LabeledField"; 
import { createFilterHash } from "./createFilterHash"; 

export default function FilterControl() {
  // Initialize the filters hashtable
  const filters = createFilterHash();

  const [formValues, setFormValues] = useState(filters);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormValues((prevState) => ({
      ...prevState,
      [name]: { ...prevState[name], value }, // Update the value in the filter hashtable
    }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    console.log(formValues); // Submit the form data
  };

  return (
    <div className="flexChildCenter" style={{ padding: 10 }}>
      <div>
        <form onSubmit={handleSubmit}>
          {Object.entries(formValues).map(([key, info]) => (
            <LabeledField
              key={key}
              info={info}
              onChange={handleChange}
              style={index !== 0 ? { marginLeft: "10px" } : {}}
            />
          ))}
          <button type="submit" style={{ marginTop: 10 }}>Submit</button>
        </form>
      </div>
    </div>
  );
}
