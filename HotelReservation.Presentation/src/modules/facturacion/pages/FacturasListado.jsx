import React, { useEffect, useState } from "react";
import { facturacionService } from "../service/facturacionService";
import { useNavigate } from "react-router-dom";
import "../styles/facturacion.css";

export default function FacturasListado() {
    const [facturas, setFacturas] = useState([]);
    const [loading, setLoading] = useState(true);
    const navigate = useNavigate();

    useEffect(() => {
        cargarFacturas();
    }, []);

    async function cargarFacturas() {
        try {
            const data = await facturacionService.listar();
            setFacturas(data);
        } catch (e) {
            console.error("Error cargando facturas", e);
        } finally {
            setLoading(false);
        }
    }

    return (
        <div className="facturacion-container">
            <h1 className="titulo">Gestion de Facturas</h1>

            <button
                className="btn-crear"
                onClick={() => navigate("/facturacion/crear")}
            >
                Crear factura
            </button>

            <div className="tabla">
                <div className="tabla-header">
                    <span>Numero</span>
                    <span>Fecha</span>
                    <span>Monto</span>
                    <span>Metodo</span>
                    <span></span>
                </div>

                {loading ? (
                    <p className="cargando">Cargando...</p>
                ) : facturas.length === 0 ? (
                    <p>No hay facturas.</p>
                ) : (
                    facturas.map((f) => (
                        <div key={f.id} className="tabla-row">
                            <span>{f.numeroFactura}</span>
                            <span>{f.fecha}</span>
                            <span>${f.montoTotal}</span>
                            <span>{f.metodoPago}</span>
                            <button
                                className="btn-detalle"
                                onClick={() =>
                                    navigate(`/facturacion/detalle/${f.id}`)
                                }
                            >
                                Detalle
                            </button>
                        </div>
                    ))
                )}
            </div>
        </div>
    );
}
