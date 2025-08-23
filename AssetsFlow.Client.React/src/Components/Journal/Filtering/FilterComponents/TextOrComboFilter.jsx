import React from "react";
import { TextInput, Select } from "@mantine/core";
import { filterTextInputPropTypes } from "@services/PropTypes/filtersPropTypes";

function TextOrComboFilter({ filter, onChange }) {
  // If restrictions exist, render Select (combo box)
  if (filter.restrictions && filter.restrictions.length > 0) {
    return (
      <Select
        placeholder={filter.title}
        value={filter.value || ""}
        onChange={(val) => onChange(filter.id, val)}
        data={filter.restrictions.map((r) => ({ label: r, value: r }))}
        searchable
        nothingFoundLabel="No options"
        clearable
        size="xs"
      />
    );
  }

  // Fallback to regular TextInput
  return (
    <TextInput
      placeholder={filter.title}
      value={filter.value || ""}
      onChange={(e) => onChange(filter.id, e.currentTarget.value)}
      size="xs"
    />
  );
}

TextOrComboFilter.propTypes = filterTextInputPropTypes;

export default React.memo(TextOrComboFilter);
