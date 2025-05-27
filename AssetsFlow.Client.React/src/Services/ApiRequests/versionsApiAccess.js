import { baseURL, request } from "./ApiRequestsWrapper";

export async function getVersions() {
  return request(`${baseURL}info`, { method: "GET" });
}
