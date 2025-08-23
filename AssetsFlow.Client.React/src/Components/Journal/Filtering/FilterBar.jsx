import { useState, useMemo } from "react";
import { Paper, Group, Button, Stack } from "@mantine/core";
import FilterSelector from "./FilterSelector";
import FilterChips from "./FilterChips";
import { useUserData } from "@hooks/UserManagement/useUserData";

export default function FilterBar({ onFiltersChange }) {
  const {
    filters: rawFilterDefinitions,
    symbols: symbolRestrictions,
    isLoading: isUserDataLoading,
  } = useUserData();

  const filterDefinitionsForUi = useMemo(() => {
    return (rawFilterDefinitions || []).map((def) =>
      def.id === "Symbol" ? { ...def, restrictions: symbolRestrictions ?? [] } : def
    );
  }, [rawFilterDefinitions, symbolRestrictions]);

  const [filters, setFilters] = useState([]);

  const addFilter = (filter) => {
    setFilters((prev) => {
      const next = prev.filter((f) => f.field !== filter.field);

      const def = filterDefinitionsForUi.find((d) => d.id === filter.field);
      const filterWithTitle = {
        ...filter,
        title: def?.title || filter.title || filter.field,
      };

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

  if (isUserDataLoading || !filterDefinitionsForUi || filterDefinitionsForUi.length === 0)
    return null;

  return (
    <Stack spacing="xs" className="mx-3 justify-start">
      <Paper shadow="xs" radius="md" className="p-2 flex items-center">
        <Group spacing="sm" align="center" className="flex-1">
          <FilterSelector
            filters={filters}
            onAdd={addFilter}
            onRemove={removeFilter}
            filterDefinitions={filterDefinitionsForUi}
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
