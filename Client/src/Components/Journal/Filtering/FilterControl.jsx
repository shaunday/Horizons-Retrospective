import React, { useState } from "react";
import LabeledField from "@components/LabeledField"; 
import { createFilterHash } from "./createFilterHash"; 

export default function FilterControl() {
  // Initialize the filters hashtable
  const filters = createFilterHash();

  // State to store the form values
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
          <LabeledField
            info={formValues.ticker}
            onChange={handleChange}
          />
          <LabeledField
            info={formValues.startDate}
            onChange={handleChange}
          />
          <LabeledField
            info={formValues.endDate}
            onChange={handleChange}
          />
          <button type="submit" style={{ marginTop: 10 }}>Submit</button>
        </form>
      </div>
    </div>
  );
}
