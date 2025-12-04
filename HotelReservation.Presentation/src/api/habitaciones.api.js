import httpClient from "./httpClient";

export const getRooms = () => httpClient.get("/Habitaciones");

export const getHabitacionById = (id) => httpClient.get(`/Habitaciones/${id}`);

export const insertHabitacion = (habitacion) =>
    httpClient.post("/Habitaciones", habitacion);

export const updateHabitacion = (id, habitacion) =>
    httpClient.put(`/Habitaciones/${id}`, habitacion);

export const deleteHabitacion = (id) => httpClient.delete(`/Habitaciones/${id}`);

