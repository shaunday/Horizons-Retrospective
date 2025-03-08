import React, { useMemo } from 'react';
import { Paper } from '@mantine/core'; 
import DataElement from '../DataElement/DataElement'; 
import { getComponentTypeStyles } from '../DataElementGroups/ComponentTypeStyles';
import * as Constants from "@constants/journalConstants"; 
import { getGroupedEntries } from './groupedEntries';

const styles = {
  listItem: {
    padding: "5px",
    minWidth: "100px",
    maxWidth: "150px",
  },
};

function GroupedEntriesList({ entries, isOverview, processCellUpdate }) {
  const groupedEntries = useMemo(() => getGroupedEntries(entries, isOverview), [entries, isOverview]);

  return (
    <>
      {Object.keys(groupedEntries).map((groupKey) => {
        const entries = groupedEntries[groupKey];
        return (
          <Paper
            radius="md"
            key={groupKey}
            style={{
              backgroundColor: !isOverview ? getComponentTypeStyles(groupKey).backgroundColor : "transparent",
            }}
          >
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