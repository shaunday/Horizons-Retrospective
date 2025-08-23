import React, { useMemo, useCallback } from "react";
import { Group } from "@mantine/core";
import { filterSelectorPropTypes } from "@services/PropTypes/filtersPropTypes";
import DateInputFilter from "./FilterComponents/DateInputFilter";
import SegmentedControlFilter from "./FilterComponents/SegmentedControlFilter";
import TextInputFilter from "./FilterComponents/TextInputFilter";

export default function FilterSelector({ filters = [], onAdd, onRemove, filterDefinitions = [] }) {
  // Memoized handleChange that works for all filter types
  const handleChange = useCallback(
    (id, value) => {
      if (value === null || value === "") onRemove(id);
      else onAdd(id, value);
    },
    [onAdd, onRemove]
  );

  const activeFilters = useMemo(
    () =>
      filterDefinitions.map((def) => {
        const current = filters.find((f) => f.field === def.id);
        return { ...def, value: current?.value ?? null };
      }),
    [filterDefinitions, filters]
  );

  if (filterDefinitions.length === 0) return null;

  return (
    <Group spacing="sm" align="center" className="flex-1">
      {activeFilters.map((filter) => {
        switch (filter.type) {
          case "Enum":
            return (
              <SegmentedControlFilter
                key={filter.id}
                filter={filter}
                onChange={handleChange}
                data={filter.restrictions.map((r) => ({
                  label: r.charAt(0).toUpperCase() + r.slice(1),
                  value: r,
                }))}
              />
            );
          case "DateRange":
            return (
              <DateInputFilter
                key={filter.id}
                filter={filter}
                onChange={handleChange}
                size="xs"
                radius="sm"
              />
            );
          case "Text":
            return <TextInputFilter key={filter.id} filter={filter} onChange={handleChange} />;
          default:
            return null;
        }
      })}
    </Group>
  );
}

FilterSelector.propTypes = filterSelectorPropTypes;
