import React from "react";
import { Select } from "@mantine/core";

function DefaultSelect({ value, onChange, data, ...otherProps }) {
  return (
    <Select
      value={value}
      onChange={onChange}
      placeholder="Select..."
      searchable
      nothingFoundMessage="Nothing found..."
      data={data}
      {...otherProps} // Pass any additional props to Select
    />
  );
}

export default DefaultSelect;
