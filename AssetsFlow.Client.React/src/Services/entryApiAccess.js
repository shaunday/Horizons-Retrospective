import axios from "axios";

const baseURL = import.meta.env.VITE_API_URL;
const componentsSuffix = import.meta.env.VITE_JOURNAL_COMPONENTS_SUFFIX;

const componentsClient = axios.create({ baseURL: `${baseURL}${componentsSuffix}` });

export async function updateEntry(componentId, newContent, changeNote) {
  const payload = {
    content: newContent,
    info: changeNote,
  };
  const response = await componentsClient.put(`${componentId}`, newContent);
  return response.data;
}
