// src/modules/facturacion/service/facturacionService.js
import httpClient from "../../../api/httpClient";

const baseUrl = "/facturacion";

export const facturacionService = {
  listar: async () => {
    const r = await httpClient.get(`${baseUrl}/listar`);
    return r.data;
  },

  crear: async (factura) => {
    const r = await httpClient.post(`${baseUrl}/crear`, factura);
    return r.data;
  },

  detalle: async (id) => {
    const r = await httpClient.get(`${baseUrl}/detalle/${id}`);
    return r.data;
  },

  generarPdf: async (id) => {
    const r = await httpClient.get(`${baseUrl}/pdf/${id}`, {
      responseType: "blob",
    });
    return r.data;
  },
};
