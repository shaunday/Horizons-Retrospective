import axios from "axios";

const baseURL = import.meta.env.VITE_API_URL;
const elementsSuffix = import.meta.env.VITE_JOURNAL_ELEMENTS_SUFFIX;

const elmentsClient = axios.create({ baseURL: `${baseURL}${elementsSuffix}` });

export async function activateElementAPI(elementId) {
  const response = await elmentsClient.patch(`${elementId}/activate`);
  return response.data;
}

export async function deleteElementAPI(elementId) {
  const response = await elmentsClient.patch(`${elementId}/activate`);
  return response.data;
}