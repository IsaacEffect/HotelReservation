import httpClient from "./httpClient";

export const getClients = () => httpClient.get("/Clientes/GetAllClients");

export const getClientById = (id) =>
  httpClient.get(`/Clientes/GetClientById/${id}`);

export const insertClient = (client) =>
  httpClient.post("/Clientes/InsertClient", client);

export const modifyClient = (id, client) =>
  httpClient.put(`/Clientes/ModifyClient/${id}`, client);

export const deleteClient = (id) =>
  httpClient.delete(`/Clientes/DeleteClient/${id}`);
