import * as Constants from "@constants/journalConstants";

export function getGroupedEntries(entries, isOverview) {
  if (isOverview) {
    return { allEntries: entries || [] }; // Fix for using entries
  }

  const groups = {};
  entries?.forEach((entry) => {
    const componentType = entry[Constants.COMPONENT_TYPE];
    if (componentType) {
      if (!groups[componentType]) {
        groups[componentType] = [];
      }
      groups[componentType].push(entry);
    }
  });

  return groups;
}