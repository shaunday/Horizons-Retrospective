import React from "react";
import { DatePickerInput } from "@mantine/dates";
import { TbCalendar } from "react-icons/tb";
import { filterDateInputPropTypes } from "@services/PropTypes/filtersPropTypes";

function DateInputFilter({ filter, onAdd, onRemove, ...props }) {
  return (
    <DatePickerInput
      placeholder={filter.title}
      value={filter.value || null}
      onChange={(val) => (val === null ? onRemove(filter.id) : onAdd(filter.id, val))}
      clearable
      rightSection={<TbCalendar size={14} />}
      {...props}
    />
  );
}

DateInputFilter.propTypes = filterDateInputPropTypes;

export default React.memo(DateInputFilter);
