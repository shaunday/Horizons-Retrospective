import React from "react";
import { Select } from "@mantine/core";

function DefaultSelect({ value, onChange, data }) {
  return (
    <Select
      value={value}
      onChange={onChange}
      placeholder="Select..."
      searchable
      nothingFoundMessage="Nothing found..."
      data={data}
    />
  );
}

export default DefaultSelect;
