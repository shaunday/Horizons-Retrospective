import React, { useState } from "react";
import { Select } from "@mantine/core";

function DataValuesSelect({ value, onChange, data }) {
  const [error, setError] = useState(null);

  const handleChange = (val) => {
    if (!val) {
      setError("Selection is required");
    } else {
      setError(null);
    }
    onChange(val);
  };

  return (
    <Select
      label="Value"
      placeholder="Pick one"
      data={data}
      value={value}
      onChange={handleChange}
      error={error}
      withAsterisk
    />
  );
}

export default React.memo(DataValuesSelect);
