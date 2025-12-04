import { useState, useEffect } from "react";
import { useCheckOut } from "../hooks/useCheckOut";
import { getAllReservationsWithDetails } from "../../../api/reservas.api";

export default function CheckOutForm() {
    const [reservas, setReservas] = useState([]);

    const [form, setForm] = useState({
        reservaId: "",
        fechaCheckOut: "",
        observaciones: "",
    });

    const { loading, error, result, submit } = useCheckOut();

    useEffect(() => {
        getAllReservationsWithDetails().then((res) => {
            const activas = res.data.filter(
                (r) =>
                    r.estadoReserva === "Pendiente" ||
                    r.estadoReserva === "Confirmada"
            );
            setReservas(activas);
        });
    }, []);

    const handleChange = (e) =>
        setForm({ ...form, [e.target.name]: e.target.value });

    const handleSubmit = (e) => {
        e.preventDefault();

        submit({
            reservaId: form.reservaId,
            fechaCheckOut:
                form.fechaCheckOut || new Date().toISOString(),
            observaciones: form.observaciones,
        });
    };

    return (
        <div className="max-w-2xl mx-auto bg-[#1A2E44] p-8 rounded-xl shadow-lg">
            <h1 className="text-2xl font-bold mb-6">Registrar Check-Out</h1>

            <form onSubmit={handleSubmit}>
                {/* Reservas */}
                <select
                    name="reservaId"
                    value={form.reservaId}
                    onChange={handleChange}
                    className="bg-[#0F1A2B] p-3 w-full rounded text-white mb-4"
                >
                    <option value="">Seleccione una reserva</option>
                    {reservas.map((r) => (
                        <option key={r.reservaId} value={r.reservaId}>
                            {r.cliente} — Hab. {r.numeroHabitacion}
                        </option>
                    ))}
                </select>

                {/* Fecha */}
                <input
                    type="datetime-local"
                    name="fechaCheckOut"
                    value={form.fechaCheckOut}
                    onChange={handleChange}
                    className="bg-[#0F1A2B] p-3 w-full rounded text-white mb-4"
                />

                {/* Observaciones */}
                <textarea
                    name="observaciones"
                    value={form.observaciones}
                    onChange={handleChange}
                    className="bg-[#0F1A2B] p-3 w-full rounded text-white mb-4"
                    placeholder="Observaciones"
                />

                <button
                    className="bg-[#FF9900] hover:bg-[#D88000] py-3 px-4 rounded w-full font-bold text-white"
                    disabled={loading}
                >
                    {loading ? "Registrando..." : "Registrar Check-Out"}
                </button>

                {result && (
                    <p className="mt-3 text-green-400 font-semibold">
                        ✔ Check-Out registrado correctamente
                    </p>
                )}
                {error && (
                    <p className="mt-3 text-red-400 font-semibold">❌ {error}</p>
                )}
            </form>
        </div>
    );
}
