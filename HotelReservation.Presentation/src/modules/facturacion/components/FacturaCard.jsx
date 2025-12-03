import React from "react";
import { useNavigate } from "react-router-dom";
import "../styles/facturacion.css";

export default function FacturaCard({ factura }) {
    const navigate = useNavigate();

    return (
        <div className="factura-card">
            <div className="factura-card-info">
                <h3 className="factura-card-numero">Factura #{factura.numeroFactura}</h3>
                <p><strong>Fecha:</strong> {factura.fecha}</p>
                <p><strong>Monto:</strong> ${factura.montoTotal}</p>
                <p><strong>Método:</strong> {factura.metodoPago}</p>
            </div>

            <button
                className="btn-detalle"
                onClick={() => navigate(`/facturacion/detalle/${factura.id}`)}
            >
                Ver Detalle
            </button>
        </div>
    );
}
