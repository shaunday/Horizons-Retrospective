import React from "react";
import { SegmentedControl } from "@mantine/core";
import { filterSegmentedControlPropTypes } from "@services/PropTypes/filtersPropTypes";

function SegmentedControlFilter({ filter, data = [], onAdd, onRemove }) {
  return (
    <SegmentedControl
      value={filter.value || ""}
      onChange={(val) => (val === filter.value ? onRemove(filter.id) : onAdd(filter.id, val))}
      data={data}
      size="xs"
    />
  );
}

SegmentedControlFilter.propTypes = filterSegmentedControlPropTypes;

export default React.memo(SegmentedControlFilter);
