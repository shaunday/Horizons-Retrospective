import React from "react";
import { SegmentedControl, Group, TextInput } from "@mantine/core";
import { DatePickerInput } from "@mantine/dates";
import { TbCalendar } from "react-icons/tb";

import {
  filterSelectorPropTypes,
  filterDateInputPropTypes,
  filterSegmentedControlPropTypes,
  filterTextInputPropTypes,
} from "@services/PropTypes/filtersPropTypes";

export default function FilterSelector({ filters = [], onAdd, onRemove, filterDefinitions = [] }) {
  const handleAdd = (field, value) => {
    if (!field || value == null) return;
    onAdd({ field, value });
  };

  const FilterDateInput = ({ filter, ...props }) => (
    <DatePickerInput
      placeholder={filter.title}
      value={filter.value || null}
      onChange={(val) => (val === null ? onRemove(filter.id) : handleAdd(filter.id, val))}
      clearable
      rightSection={<TbCalendar size={14} />}
      {...props}
    />
  );
  FilterDateInput.propTypes = filterDateInputPropTypes;

  const FilterSegmentedControl = ({ filter, data = [] }) => (
    <SegmentedControl
      value={filter.value || null}
      onChange={(val) => (val === filter.value ? onRemove(filter.id) : handleAdd(filter.id, val))}
      data={data}
      size="xs"
    />
  );
  FilterSegmentedControl.propTypes = filterSegmentedControlPropTypes;

  const FilterTextInput = ({ filter }) => (
    <TextInput
      placeholder={filter.title}
      value={filter.value || ""}
      onChange={(e) => {
        const val = e.currentTarget.value;
        val ? handleAdd(filter.id, val) : onRemove(filter.id);
      }}
      size="xs"
    />
  );
  FilterTextInput.propTypes = filterTextInputPropTypes;

  const activeFilters = filterDefinitions.map((def) => {
    const current = filters.find((f) => f.field === def.id);
    return {
      ...def,
      value: current?.value ?? null,
    };
  });

  return (
    <Group spacing="sm" noWrap align="center" className="flex-1">
      {activeFilters.map((filter) => {
        switch (filter.type) {
          case "enum":
            return (
              <FilterSegmentedControl
                key={filter.id}
                filter={filter}
                data={filter.restrictions.map((r) => ({
                  label: r.charAt(0).toUpperCase() + r.slice(1),
                  value: r,
                }))}
              />
            );
          case "dateRange":
            return <FilterDateInput key={filter.id} filter={filter} size="xs" radius="sm" />;
          case "text":
            return <FilterTextInput key={filter.id} filter={filter} />;
          default:
            return null;
        }
      })}
    </Group>
  );
}

FilterSelector.propTypes = filterSelectorPropTypes;
