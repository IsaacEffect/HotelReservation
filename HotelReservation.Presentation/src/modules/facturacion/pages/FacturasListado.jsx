import React, { useEffect, useState } from "react";
import { facturacionService } from "../service/facturacionService";
import { useNavigate } from "react-router-dom";
import "../styles/facturasListado.css";

export default function FacturasListado() {
    const [facturas, setFacturas] = useState([]);
    const [loading, setLoading] = useState(false);
    const navigate = useNavigate();

    // Filtros
    const [filtros, setFiltros] = useState({
        fechaDesde: "",
        fechaHasta: "",
        metodoPago: "",
        huesped: "",
        montoMin: "",
        montoMax: ""
    });

    useEffect(() => {
        obtenerFacturas();
    }, []);

    async function obtenerFacturas() {
        setLoading(true);
        try {
            const data = await facturacionService.listar(filtros);
            setFacturas(data);
        } catch (error) {
            console.error("Error obteniendo facturas:", error);
        } finally {
            setLoading(false);
        }
    }

    const handleChange = (e) => {
        setFiltros({ ...filtros, [e.target.name]: e.target.value });
    };

    const limpiar = async () => {
        const reset = {
            fechaDesde: "",
            fechaHasta: "",
            metodoPago: "",
            huesped: "",
            montoMin: "",
            montoMax: ""
        };
        setFiltros(reset);
        const data = await facturacionService.listar(reset);
        setFacturas(data);
    };

    return (
        <div className="facturas-container">

            {/* BOTÓN VOLVER */}
            <button 
                onClick={() => navigate("/")} 
                className="btn-blanco"
                style={{ marginBottom: "15px" }}
            >
                ← Volver
            </button>
            
            <h2 className="titulo">Listado de Facturas</h2>

            {/* FILTROS */}
            <div className="filtros">
                <div>
                    <label>Desde:</label>
                    <input name="fechaDesde" type="date" value={filtros.fechaDesde} onChange={handleChange} />
                </div>

                <div>
                    <label>Hasta:</label>
                    <input name="fechaHasta" type="date" value={filtros.fechaHasta} onChange={handleChange} />
                </div>

                <div>
                    <label>Metodo de pago:</label>
                    <select name="metodoPago" value={filtros.metodoPago} onChange={handleChange}>
                        <option value="">Todos</option>
                        <option value="Efectivo">Efectivo</option>
                        <option value="Tarjeta">Tarjeta</option>
                        <option value="Transferencia">Transferencia</option>
                    </select>
                </div>

                <div>
                    <label>Huesped:</label>
                    <input name="huesped" type="text" placeholder="Nombre" value={filtros.huesped} onChange={handleChange} />
                </div>

                <div>
                    <label>Monto Min:</label>
                    <input name="montoMin" type="number" value={filtros.montoMin} onChange={handleChange} />
                </div>

                <div>
                    <label>Monto Max:</label>
                    <input name="montoMax" type="number" value={filtros.montoMax} onChange={handleChange} />
                </div>

                <div className="botones">
                    <button onClick={obtenerFacturas} className="btn-amarillo">Filtrar</button>
                    <button onClick={limpiar} className="btn-blanco">Limpiar</button>
                </div>
            </div>

            {/* LISTADO */}
            <div className="tabla">
                <table>
                    <thead>
                        <tr>
                            <th>ID</th>
                            <th>Fecha</th>
                            <th>Cliente</th>
                            <th>Metodo</th>
                            <th>Total</th>
                            <th></th>
                        </tr>
                    </thead>

                    <tbody>
                        {loading ? (
                            <tr><td colSpan="6" style={{ textAlign: "center" }}>Cargando...</td></tr>
                        ) : facturas.length === 0 ? (
                            <tr><td colSpan="6" style={{ textAlign: "center" }}>No hay resultados</td></tr>
                        ) : (
                            facturas.map((f) => (
                                <tr key={f.id}>
                                    <td>{f.id}</td>
                                    <td>{new Date(f.fechaEmision).toLocaleDateString()}</td>
                                    <td>{f.huespedNombre}</td>
                                    <td>{f.metodoPago}</td>
                                    <td>RD${f.montoTotal}</td>
                                    <td>
                                        <a className="ver-detalle" href={`/facturacion/detalle/${f.id}`}>
                                            Ver
                                        </a>
                                    </td>
                                </tr>
                            ))
                        )}
                    </tbody>

                </table>
            </div>
        </div>
    );
}

