import React from 'react';
import DataElement from '../DataElement/DataElement';

function EntriesList({ entries, processCellUpdate, overviewType }) {
  return (
    <ul>
      {entries.map((entry) => (
        <li key={entry.id} style={{padding:"3px"}}>
          <DataElement
            cellInfo={entry}
            onCellUpdate={processCellUpdate}
            overviewType={overviewType}
          />
        </li>
      ))}
    </ul>
  );
}

export default React.memo(EntriesList);