import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { facturacionService } from "../service/facturacionService";
import "../styles/facturacion.css";

export default function FacturaDetalle() {
    const { id } = useParams();
    const [factura, setFactura] = useState(null);

    useEffect(() => {
        cargar();
    }, []);

    async function cargar() {
        const data = await facturacionService.detalle(id);
        setFactura(data);
    }

    if (!factura) return <p>Cargando...</p>;

    return (
        <div className="facturacion-container">
            <h1 className="titulo">Detalle de Factura</h1>

            <div className="detalle-box">
                <p><strong>Número:</strong> {factura.numeroFactura}</p>
                <p><strong>Fecha:</strong> {factura.fecha}</p>
                <p><strong>Monto:</strong> ${factura.montoTotal}</p>
                <p><strong>Método:</strong> {factura.metodoPago}</p>
            </div>
        </div>
    );
}
