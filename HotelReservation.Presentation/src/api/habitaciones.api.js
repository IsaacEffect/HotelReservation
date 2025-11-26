import httpClient from "./httpClient";

export const getRooms = () => httpClient.get("/Habitaciones");
