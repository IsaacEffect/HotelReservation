import httpClient from "./httpClient";

export const loginRequest = async ({ correo, contrasena }) => {
  const { data } = await httpClient.post("/Auth/Login", {
    correo,
    contrasena,
  });
  return data;
};
