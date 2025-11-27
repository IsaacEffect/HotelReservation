import httpClient from "./httpClient";

export const getAllReservations = () =>
  httpClient.get("/Reservas/GetAllReservations");

export const createReservation = (reservation) =>
  httpClient.post("/Reservas/CreateReservation", reservation);

export const getReservationById = (id) =>
  httpClient.get(`/Reservas/GetReservationById/${id}`);

export const getAllReservationsWithDetails = () =>
  httpClient.get("/Reservas/GetAllReservationsWithDetails");

export const getReservationDetailsById = (id) =>
  httpClient.get(`/Reservas/GetReservationDetailsById/${id}`);

export const updateReservation = (id, reservation) =>
  httpClient.put(`/Reservas/UpdateReservation/${id}`, reservation);

export const updateReservationStatus = (id, statusDto) =>
  httpClient.patch(`/Reservas/UpdateReservationStatus/${id}`, statusDto);

export const cancelReservation = (id) =>
  httpClient.delete(`/Reservas/CancelReservation/${id}`);

export const checkRoomAvailability = ({ habitacionId, fechaInicio, fechaFin }) =>
  httpClient.get(
    `/Reservas/CheckHabitacionDisponibilidad?habitacionId=${habitacionId}&fechaInicio=${fechaInicio}&fechaFin=${fechaFin}`
  );


