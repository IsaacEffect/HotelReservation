import React, { useState } from "react";
import { facturacionService } from "../service/facturacionService";
import { useNavigate } from "react-router-dom";
import ErrorMessage from "../components/ErrorMessage";
import "../styles/facturacion.css";

export default function FacturaCrear() {
    const navigate = useNavigate();
    const [error, setError] = useState("");

    const [form, setForm] = useState({
        reservaId: "",
        metodoPago: "",
    });

    function handleChange(e) {
        setForm({ ...form, [e.target.name]: e.target.value });
    }

    async function handleSubmit(e) {
        e.preventDefault();

        if (!form.reservaId || !form.metodoPago) {
            setError("Todos los campos son obligatorios");
            return;
        }

        try {
            await facturacionService.crear(form);
            navigate("/facturacion");
        } catch {
            setError("Error creando factura");
        }
    }

    return (
        <div className="facturacion-container">
            <h1 className="titulo">Crear Factura</h1>

            {error && <ErrorMessage mensaje={error} />}

            <form className="formulario" onSubmit={handleSubmit}>
                <label>Reserva ID</label>
                <input
                    name="reservaId"
                    className="input"
                    value={form.reservaId}
                    onChange={handleChange}
                />

                <label>Método de Pago</label>
                <select
                    className="input"
                    name="metodoPago"
                    value={form.metodoPago}
                    onChange={handleChange}
                >
                    <option value="">Seleccione un método...</option>
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
