import { baseURL, request } from "./ApiRequestsWrapper";

const elementsBaseURL = `${baseURL}${import.meta.env.VITE_JOURNAL_ELEMENTS_SUFFIX}`;

export async function activateElementAPI(elementId) {
  return request(`${elementsBaseURL}/${elementId}/activate`, { method: "PATCH" });
}

export async function deleteElementAPI(elementId) {
  return request(`${elementsBaseURL}/${elementId}`, { method: "DELETE" });
}