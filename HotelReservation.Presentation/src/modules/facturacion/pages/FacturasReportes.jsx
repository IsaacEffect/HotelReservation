import React, { useEffect, useState } from "react";
import { facturacionService } from "../service/facturacionService";
import { useNavigate } from "react-router-dom";
import "../styles/facturacion.css";

export default function FacturasReportes() {
    const [facturas, setFacturas] = useState([]);
    const [filtradas, setFiltradas] = useState([]);
    const [filtroFecha, setFiltroFecha] = useState("");
    const [filtroMetodo, setFiltroMetodo] = useState("");
    const navigate = useNavigate();

    useEffect(() => {
        cargarFacturas();
    }, []);

    async function cargarFacturas() {
        try {
            const data = await facturacionService.listar();

            // Ajustar campos segun tu JSON real
            const normalizadas = data.map(f => ({
                id: f.id,
                numeroFactura: f.id?.substring(0, 6) ?? "N/A",
                fecha: f.fechaEmision?.split("T")[0] ?? "N/A",
                montoTotal: f.montoTotal ?? 0,
                metodoPago: f.metodoPago ?? "N/A",
            }));

            setFacturas(normalizadas);
            setFiltradas(normalizadas);
        } catch (error) {
            console.error("Error cargando facturas:", error);
        }
    }

    function aplicarFiltros() {
        let lista = [...facturas];

        if (filtroFecha) {
            lista = lista.filter(f => f.fecha === filtroFecha);
        }

        if (filtroMetodo) {
            lista = lista.filter(f => f.metodoPago === filtroMetodo);
        }

        setFiltradas(lista);
    }

    function limpiarFiltros() {
        setFiltroFecha("");
        setFiltroMetodo("");
        setFiltradas(facturas);
    }

    return (
        <div className="facturacion-container">
            {/* BOTÓN VOLVER */}
            <button 
                onClick={() => navigate("/")} 
                className="btn-blanco"
                style={{ marginBottom: "15px" }}
            >
                ← Volver
            </button>

            <h1 className="titulo">Reportes de Facturacion</h1>

            {/* FILTROS */}
            <div className="reporte-filtros" style={{ marginBottom: "30px" }}>
                <div>
                    <label>Fecha:</label>
                    <input
                        type="date"
                        value={filtroFecha}
                        onChange={(e) => setFiltroFecha(e.target.value)}
                    />
                </div>

                <div>
                    <label>Metodo de Pago:</label>
                    <select
                        value={filtroMetodo}
                        onChange={(e) => setFiltroMetodo(e.target.value)}
                    >
                        <option value="">Todos</option>
                        <option>Efectivo</option>
                        <option>Tarjeta</option>
                        <option>Transferencia</option>
                    </select>
                </div>

                <button className="btn-crear" onClick={aplicarFiltros}>
                    Filtrar
                </button>

                <button
                    className="btn-limpiar"
                    onClick={limpiarFiltros}
                    style={{ background: "white", color: "black", fontWeight: "bold" }}
                >
                    Limpiar
                </button>
            </div>

            {/* TABLA SIMPLE */}
            <div className="tabla">
                <div className="tabla-header">
                    <span>Numero</span>
                    <span>Fecha</span>
                    <span>Monto</span>
                    <span>Metodo</span>
                </div>

                {filtradas.length === 0 ? (
                    <p style={{ padding: "15px" }}>No hay facturas registradas.</p>
                ) : (
                    filtradas.map((f) => (
                        <div key={f.id} className="tabla-row">
                            <span>{f.numeroFactura}</span>
                            <span>{f.fecha}</span>
                            <span>${f.montoTotal}</span>
                            <span>{f.metodoPago}</span>
                        </div>
                    ))
                )}
            </div>
        </div>
    );
}

