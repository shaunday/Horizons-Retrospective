// FilterBar.jsx
import { useState } from "react";
import { Paper, Group, Button, Stack } from "@mantine/core";
import FilterSelector from "./FilterSelector";
import FilterChips from "./FilterChips";

// Move filter definitions here
const filterDefinitions = [
  { id: "wl", title: "W/L", type: "enum", restrictions: ["win", "loss"] },
  { id: "status", title: "Status", type: "enum", restrictions: ["idea", "open", "closed"] },
  { id: "symbol", title: "Symbol", type: "text", restrictions: [] },
  { id: "openDateRange", title: "Open Date", type: "dateRange", restrictions: [] },
  { id: "closeDateRange", title: "Close Date", type: "dateRange", restrictions: [] },
];

export default function FilterBar({ onFiltersChange }) {
  const [filters, setFilters] = useState([]);

  const addFilter = (filter) => {
    setFilters((prev) => {
      const next = prev.filter((f) => f.field !== filter.field);

      // Always carry the title from definitions
      const def = filterDefinitions.find((d) => d.id === filter.field);
      const filterWithTitle = { ...filter, title: def?.title || filter.title || filter.field };

      next.push(filterWithTitle);
      onFiltersChange?.(next);
      return next;
    });
  };

  const removeFilter = (fieldOrId) => {
    setFilters((prev) => {
      const next = prev.filter((f) => f.id !== fieldOrId && f.field !== fieldOrId);
      onFiltersChange?.(next);
      return next;
    });
  };

  const clearFilters = () => {
    setFilters([]);
    onFiltersChange?.([]);
  };

  return (
    <Stack spacing="xs" className="mx-3 justify-start">
      <Paper shadow="xs" radius="md" className="p-2 flex items-center">
        <Group spacing="sm" noWrap align="center" className="flex-1">
          <FilterSelector
            filters={filters}
            onAdd={addFilter}
            onRemove={removeFilter}
            filterDefinitions={filterDefinitions}
          />
          <Button size="xs" variant="outline" color="red" onClick={clearFilters}>
            Clear Filters
          </Button>
        </Group>
      </Paper>
      <FilterChips filters={filters} onRemove={removeFilter} />
    </Stack>
  );
}
