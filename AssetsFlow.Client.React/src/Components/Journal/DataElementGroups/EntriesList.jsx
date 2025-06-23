import React from "react";
import DataElement from "../DataElement/DataElement";

function EntriesList({ entries, overviewType }) {
  return (
    <ul className="flex flex-wrap gap-1">
      {entries.map((entry) => (
        <li key={entry.id} className="p-1">
          <DataElement cellInfo={entry} overviewType={overviewType} />
        </li>
      ))}
    </ul>
  );
}

export default React.memo(EntriesList);
