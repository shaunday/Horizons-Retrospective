import React from 'react';
import DataElement from '../DataElement/DataElement';

const styles = {
  listItem: {
    padding: "3px",
    minWidth: "90px",
    maxWidth: "150px",
  },
};

function EntriesList({ entries, processCellUpdate }) {
  return (
    <ul>
      {entries.map((entry) => (
        <li key={entry.id} style={styles.listItem}>
          <DataElement
            cellInfo={entry}
            onCellUpdate={processCellUpdate}
          />
        </li>
      ))}
    </ul>
  );
}

export default EntriesList;