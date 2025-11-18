import httpClient from "./httpClient";

export const getRoles = () => httpClient.get("/Roles/GetAllRoles");

export const getRoleById = (id) =>
  httpClient.get(`/Roles/GetRoleById/${id}`);

export const insertRole = (role) =>
  httpClient.post("/Roles/InsertRole", role);
