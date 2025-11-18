import axios from "axios";

const httpClient = axios.create({
  baseURL: "https://localhost:7284/api",
  headers: {
    "Content-Type": "application/json",
  },
  withCredentials: false
});

// Interceptor para enviar JWT
httpClient.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

export default httpClient;
