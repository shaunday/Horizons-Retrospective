import { baseURL, request } from "./ApiRequestsWrapper";

const userDataURL = `${baseURL}${import.meta.env.VITE_JOURNAL_USERDATA_SUFFIX}`;

export async function login({ email, password } = {}) {
  const body = email && password ? { email, password } : {};
  const response = await request(`${baseURL}auth/login`, {
    method: "POST",
    body: JSON.stringify(body),
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

export function logout() {
  // No side effects here; handled in useAuth
}

export async function getUserData() {
  const response = await request(`${userDataURL}`, {
    method: "GET",
  });
  return response;
}
