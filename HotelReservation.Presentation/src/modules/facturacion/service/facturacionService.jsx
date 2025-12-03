// src/modules/facturacion/service/facturacionService.js
import axios from "axios";

const API_URL = "https://localhost:7284/api/facturacion";

export const facturacionService = {
    listar: async () => {
        const r = await axios.get(`${API_URL}/listar`);
        return r.data;
    },

    crear: async (factura) => {
        const r = await axios.post(`${API_URL}/crear`, factura);
        return r.data;
    },

    detalle: async (id) => {
        const r = await axios.get(`${API_URL}/detalle/${id}`);
        return r.data;
    },

    generarPdf: async (id) => {
        const r = await axios.get(`${API_URL}/pdf/${id}`, {
            responseType: "blob",
        });
        return r.data;
    },
};
