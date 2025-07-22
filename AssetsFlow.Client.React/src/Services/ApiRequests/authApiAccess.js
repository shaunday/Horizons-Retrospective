import { baseURL, request } from "./ApiRequestsWrapper";

export async function login({ email, password }) {
  const response = await request(`${baseURL}auth/login`, {
    method: "POST",
    body: JSON.stringify({ email, password }),
  });
  return response;
}

export async function register({ email, password, firstName, lastName }) {
  const response = await request(`${baseURL}auth/register`, {
    method: "POST",
    body: JSON.stringify({ email, password, firstName, lastName }),
  });
  return response;
}

export async function loginAsDemo() {
  const response = await request(`${baseURL}auth/demo`, {
    method: "POST",
  });
  return response;
}

export function logout() {
  // No side effects here; handled in useAuth
} 