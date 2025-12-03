import httpClient from "./httpClient";

export const getFacturas = () => httpClient.get("/Facturacion/listar");

export const getFacturaDetalle = (id) =>
  httpClient.get(`/Facturacion/detalle/${id}`);

export const crearFactura = (facturaData) =>
  httpClient.post("/Facturacion/crear", facturaData);

export const getFacturaPdf = (id) =>
  httpClient.get(`/Facturacion/pdf/${id}`, {
    responseType: "blob",
  });
