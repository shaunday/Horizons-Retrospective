import React, { useState } from "react";
import LabeledField from "@components/LabeledField"; 
import { createFilterHash } from "./createFilterHash"; 
import clsx from "clsx";

function FilterControl() {
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
    <div>
        <form onSubmit={handleSubmit}>
          {Object.entries(formValues).map(([key, info], index) => (
            <LabeledField
              key={key}
              info={info}
              onChange={handleChange}
              className={clsx({
                "ml-2.5": index !== 0
              })}
            />
          ))}
          <button type="submit" className="mt-2.5">Submit</button>
        </form>
      </div>
  );
}

export default React.memo(FilterControl);
