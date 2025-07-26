import { baseURL, request } from "./ApiRequestsWrapper";

const adminBaseURL = `${baseURL}admin`;

export async function reseedDemoUserAPI() {
  return request(`${adminBaseURL}/reseed-demo-user`, {
    method: "POST",
  });
}
