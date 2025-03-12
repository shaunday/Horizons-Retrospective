import { baseURL, request } from "./ApiRequestsWrapper";

const elementsBaseURL = `${baseURL}${import.meta.env.VITE_JOURNAL_ELEMENTS_SUFFIX}`;

export async function timestampElementAPI(elementId) {
  return request(`${elementsBaseURL}/${elementId}/timestamp`, { method: "PATCH" });
}

export async function deleteElementAPI(elementId) {
  return request(`${elementsBaseURL}/${elementId}`, { method: "DELETE" });
}