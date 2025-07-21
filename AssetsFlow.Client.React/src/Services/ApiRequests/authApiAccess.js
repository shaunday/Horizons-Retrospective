import { baseURL, request } from "./ApiRequestsWrapper";

export async function login({ email, password }) {
  const response = await request(`${baseURL}auth/login`, {
    method: "POST",
    body: JSON.stringify({ email, password }),
  });
  if (response.token) {
    localStorage.setItem("jwtToken", response.token);
  }
  return response;
}

export async function register({ email, password, firstName, lastName }) {
  const response = await request(`${baseURL}auth/register`, {
    method: "POST",
    body: JSON.stringify({ email, password, firstName, lastName }),
  });
  if (response.token) {
    localStorage.setItem("jwtToken", response.token);
  }
  return response;
}

export function logout() {
  localStorage.removeItem("jwtToken");
} 