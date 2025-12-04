import httpClient from "./httpClient";

export const getReporteIngresos = (desde, hasta) =>
  httpClient.get("/Reportes/ingresos", {
    params: {
      desde,
      hasta,
    },
  });

export const getReporteOcupacion = (desde, hasta) =>
  httpClient.get("/Reportes/ocupacion", {
    params: {
      desde,
      hasta,
    },
  });

export const getReporteOcupacionDiaria = () =>
  httpClient.get("/Reportes/ocupacion-diaria");
