import { useMemo } from 'react';
import { Box, ThemeIcon } from '@mantine/core';
import { getGroupedEntries } from './groupedEntries';
import { getComponentTypeIcon } from './componentTypeIcons';
import EntriesList from './EntriesList';
import * as Constants from "@constants/journalConstants";

const styles = {
  box: {
    // border and borderRadius moved to className
  },
  themeIcon: {
    // position, top, and left moved to className
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
            className="py-2 px-2 pl-3 m-1 relative border border-gray-300 rounded-lg"
          >
            <ThemeIcon
              variant="light"
              size="md"
              radius="lg"
              className="absolute top-10 -left-4 rounded-lg"
            >
              <IconComponent size={20} />
            </ThemeIcon>
            <EntriesList entries={entries} processCellUpdate={processCellUpdate} overviewType={Constants.OverviewType.NONE}/>
          </Box>
        );
      })}
    </>
  );
}

export default GroupedEntriesList;