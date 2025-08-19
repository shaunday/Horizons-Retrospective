import React from "react";
import { Badge, Group, ActionIcon } from "@mantine/core";
import { TbX } from "react-icons/tb";
import PropTypes from "prop-types";

export default function FilterChips({ filters, onRemove }) {
  if (!Array.isArray(filters) || filters.length === 0) return null;

  return (
    <Group spacing="xs" align="center" noWrap className="flex-nowrap overflow-x-auto">
      {filters.map((f) => (
        <Badge
          key={f.id || f.field}
          color="blue"
          variant="light"
          radius="md"
          className="whitespace-nowrap"
          rightSection={
            onRemove && (
              <ActionIcon size="xs" onClick={() => onRemove(f.field)}>
                <TbX size={12} />
              </ActionIcon>
            )
          }
        >
          {f.title}: {f.value}
        </Badge>
      ))}
    </Group>
  );
}

FilterChips.propTypes = {
  filters: PropTypes.arrayOf(
    PropTypes.shape({
      id: PropTypes.oneOfType([PropTypes.string, PropTypes.number]),
      field: PropTypes.string.isRequired,
      title: PropTypes.string.isRequired,
      value: PropTypes.any,
    })
  ),
  onRemove: PropTypes.func,
};

FilterChips.defaultProps = {
  filters: [],
  onRemove: null,
};
