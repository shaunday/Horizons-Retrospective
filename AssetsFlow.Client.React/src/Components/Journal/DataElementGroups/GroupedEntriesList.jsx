import React, { useMemo } from 'react';
import { Paper, ThemeIcon } from '@mantine/core';
import { getGroupedEntries } from './groupedEntries';
import DataElement from '../DataElement/DataElement';
import { getComponentTypeIcon } from './componentTypeIcons';

const styles = {
  listItem: {
    padding: "3px",
    minWidth: "100px",
    maxWidth: "150px",
  },
  paper: {
    position: 'relative',
    background: 'transparent'
  },
  themeIcon: {
    position: 'absolute',
    top: 40,
    left: -15,
  },
};

function GroupedEntriesList({ entries, isOverview, processCellUpdate }) {
  const groupedEntries = useMemo(() => getGroupedEntries(entries, isOverview), [entries, isOverview]);

  return (
    <>
      {Object.keys(groupedEntries).map((groupKey) => {
        const entries = groupedEntries[groupKey];
        const IconComponent = getComponentTypeIcon(groupKey);

        return (
          <Paper
            radius="md"
            p={10}
            mr={10}
            key={groupKey}
            withBorder
            style={styles.paper}
          >
            {!isOverview && <ThemeIcon
              variant="light"
              size="md"
              radius="lg"
              style={styles.themeIcon}
            >
              <IconComponent size={20} />
            </ThemeIcon>}
            <ul>
              {entries.map((entry) => (
                <li key={entry.id} style={styles.listItem}>
                  <DataElement
                    cellInfo={entry}
                    {...(!isOverview && { onCellUpdate: processCellUpdate })}
                  />
                </li>
              ))}
            </ul>
          </Paper>
        );
      })}
    </>
  );
}

export default GroupedEntriesList;