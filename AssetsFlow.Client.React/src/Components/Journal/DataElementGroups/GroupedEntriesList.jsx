import { useMemo } from 'react';
import { Box, ThemeIcon } from '@mantine/core';
import { getGroupedEntries } from './groupedEntries';
import { getComponentTypeIcon } from './componentTypeIcons';
import EntriesList from './EntriesList';
import * as Constants from "@constants/journalConstants";

function GroupedEntriesList({ entries }) {
  const groupedEntries = useMemo(() => getGroupedEntries(entries), [entries]);

  return (
    <div className="flex flex-wrap items-center gap-6">
      {Object.keys(groupedEntries).map((groupKey) => {
        const entries = groupedEntries[groupKey];
        const IconComponent = getComponentTypeIcon(groupKey);

        return (
          <Box
            key={groupKey}
            className="py-1 pr-2 pl-3 m-1 relative border border-gray-300 rounded-lg"
          >
            <ThemeIcon
              variant="light"
              size="md"
              radius="lg"
              className="absolute top-7 -left-4 rounded-lg"
            >
              <IconComponent size={20} />
            </ThemeIcon>
            <EntriesList entries={entries} overviewType={Constants.OverviewType.NONE}/>
          </Box>
        );
      })}
    </div>
  );
}

export default GroupedEntriesList;