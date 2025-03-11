import { baseURL, request } from "./ApiRequestsWrapper";

const componentsBaseURL = `${baseURL}${import.meta.env.VITE_JOURNAL_COMPONENTS_SUFFIX}`;

export async function updateEntry(componentId, newContent, changeNote) {
  return request(`${componentsBaseURL}/${componentId}`, {
    method: "PATCH",
    body: JSON.stringify({
      content: newContent,
      info: changeNote,
    }),
  });
}