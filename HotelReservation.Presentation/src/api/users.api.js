import httpClient from "./httpClient";

export const getUsers = () => httpClient.get("/Usuarios/GetAllUsers");

export const getUserById = (id) =>
  httpClient.get(`/Usuarios/GetUserById/${id}`);

export const insertUser = (user) =>
  httpClient.post("/Usuarios/InsertUser", user);

export const updateUser = (id, user) =>
  httpClient.put(`/Usuarios/UpdateUser/${id}`, user);

export const changePassword = (data) =>
  httpClient.put("/Usuarios/ChangePassword", data);

export const deleteUser = (id) =>
  httpClient.delete(`/Usuarios/DeleteUser/${id}`);
