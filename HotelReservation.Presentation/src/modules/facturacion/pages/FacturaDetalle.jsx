import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { facturacionService } from "../service/facturacionService";
import "../styles/facturaRecibo.css"; // Archivo nuevo de estilos

export default function FacturaDetalle() {
    const { id } = useParams();
    const [factura, setFactura] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        cargarFactura();
    }, []);

    async function cargarFactura() {
        try {
            const data = await facturacionService.detalle(id);
            setFactura(data);
        } catch (error) {
            console.error("Error cargando factura:", error);
        } finally {
            setLoading(false);
        }
    }

    async function handlePDF() {
        const blob = await facturacionService.generarPdf(id);
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement("a");
        a.href = url;
        a.download = `Factura-${id}.pdf`;
        a.click();
    }

    if (loading) return <p style={{ color: "white" }}>Cargando...</p>;
    if (!factura) return <p style={{ color: "white" }}>Factura no encontrada</p>;

    return (
        <div className="factura-wrapper">
            <div className="factura-recibo">
                <h2 className="factura-titulo">FACTURA</h2>

                <div className="factura-info">
                    <p><strong>Número:</strong> {factura.id}</p>
                    <p><strong>Fecha:</strong> {new Date(factura.fechaEmision).toLocaleString()}</p>
                    <p><strong>Cliente:</strong> {factura.huespedNombre || "N/D"}</p>
                    <p><strong>Método de pago:</strong> {factura.metodoPago}</p>
                </div>

                <hr />

                <h3 className="detalle-titulo">Detalles</h3>
                <div className="factura-detalles">
                    {factura.detalles?.map((d, i) => (
                        <div key={i} className="detalle-row">
                            <span>{d.descripcion}</span>
                            <span>{d.cantidad} x RD${d.precioUnitario}</span>
                            <span className="detalle-subtotal">RD${d.subtotal}</span>
                        </div>
                    ))}
                </div>

                <hr />

                <div className="factura-total">
                    <strong>Total:</strong>
                    <span>RD${factura.montoTotal}</span>
                </div>
            </div>

            <div className="acciones">
                <button className="btn-amarillo" onClick={() => window.print()}>
                    Imprimir
                </button>

                <button className="btn-blanco" onClick={handlePDF}>
                    Descargar PDF
                </button>
            </div>
        </div>
    );
}
