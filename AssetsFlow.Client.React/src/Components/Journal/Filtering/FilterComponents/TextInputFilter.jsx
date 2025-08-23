import React from "react";
import { TextInput } from "@mantine/core";
import { filterTextInputPropTypes } from "@services/PropTypes/filtersPropTypes";

function TextInputFilter({ filter, onAdd, onRemove }) {
  return (
    <TextInput
      placeholder={filter.title}
      value={filter.value || ""}
      onChange={(e) => {
        const val = e.currentTarget.value;
        val ? onAdd(filter.id, val) : onRemove(filter.id);
      }}
      size="xs"
    />
  );
}

TextInputFilter.propTypes = filterTextInputPropTypes;

export default React.memo(TextInputFilter);
