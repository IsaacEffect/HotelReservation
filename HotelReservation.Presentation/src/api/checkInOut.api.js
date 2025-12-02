import httpClient from "./httpClient";

export const registerCheckIn = (data) =>
    httpClient.post("/CheckInOut/checkin", data);

export const registerCheckOut = (data) =>
    httpClient.post("/CheckInOut/checkout", data);

export const getHistory = () =>
    httpClient.get("/CheckInOut/history");

export const getHistoryByClient = (id) =>
    httpClient.get(`/CheckInOut/history/client/${id}`);

export const getHistoryByRoom = (id) =>
    httpClient.get(`/CheckInOut/history/room/${id}`);

export const getByReservation = (id) =>
    httpClient.get(`/CheckInOut/${id}`);

export const updateHistoryRecord = (id, data) =>
    httpClient.put(`/CheckInOut/update/${id}`, data);

export const deleteHistoryRecord = (id) =>
    httpClient.delete(`/CheckInOut/delete/${id}`);

