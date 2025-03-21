import React, { useMemo } from 'react';
import { Box, ThemeIcon } from '@mantine/core';
import { getGroupedEntries } from './groupedEntries';
import { getComponentTypeIcon } from './componentTypeIcons';
import EntriesList from './EntriesList';

const styles = {
  box: {
    position: 'relative',
    border: '1px solid #ccc', 
    borderRadius: '8px', // Match Mantine's md radius
    padding: '7px 7px 7px 13px', // top, right, bottom, left
    margin: '5px',
  },
  themeIcon: {
    position: 'absolute',
    top: 40,
    left: -15,
  },
};

function GroupedEntriesList({ entries, processCellUpdate }) {
  const groupedEntries = useMemo(() => getGroupedEntries(entries), [entries]);

  return (
    <>
      {Object.keys(groupedEntries).map((groupKey) => {
        const entries = groupedEntries[groupKey];
        const IconComponent = getComponentTypeIcon(groupKey);

        return (
          <Box
            key={groupKey}
            style={styles.box}  // Applying the extracted styles
          >
            <ThemeIcon
              variant="light"
              size="md"
              radius="lg"
              style={styles.themeIcon}
            >
              <IconComponent size={20} />
            </ThemeIcon>
            <EntriesList entries={entries} processCellUpdate={processCellUpdate} />
          </Box>
        );
      })}
    </>
  );
}

export default GroupedEntriesList;