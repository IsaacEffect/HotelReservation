import httpClient from "./httpClient";

export const getCategorias = () => httpClient.get("/Categorias");

export const getCategoriaById = (id) => httpClient.get(`/Categorias/${id}`);

export const insertCategoria = (categoria) =>
    httpClient.post("/Categorias", categoria);

export const updateCategoria = (id, categoria) =>
    httpClient.put(`/Categorias/${id}`, categoria);

export const deleteCategoria = (id) => httpClient.delete(`/Categorias/${id}`);
