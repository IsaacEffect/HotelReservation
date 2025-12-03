import React, { useState } from "react";
import { facturacionService } from "../service/facturacionService";
import { useNavigate } from "react-router-dom";
import "../styles/facturacion.css";

export default function FacturaCrear() {
    const navigate = useNavigate();

    const [form, setForm] = useState({
        reservaId: "",
        metodoPago: "",
    });

    function handleChange(e) {
        setForm({ ...form, [e.target.name]: e.target.value });
    }

    async function handleSubmit(e) {
        e.preventDefault();
        await facturacionService.crear(form);
        navigate("/facturacion");
    }

    return (
        <div className="facturacion-container">
            <h1 className="titulo">Crear Factura</h1>

            <form className="formulario" onSubmit={handleSubmit}>
                <label>Reserva Id</label>
                <input
                    name="reservaId"
                    value={form.reservaId}
                    onChange={handleChange}
                />

                <label>Método de Pago</label>
                <select
                    name="metodoPago"
                    value={form.metodoPago}
                    onChange={handleChange}
                >
                    <option value="">Seleccione...</option>
                    <option>Efectivo</option>
                    <option>Tarjeta</option>
                    <option>Transferencia</option>
                </select>

                <button className="btn-crear" type="submit">
                    Guardar
                </button>
            </form>
        </div>
    );
}
