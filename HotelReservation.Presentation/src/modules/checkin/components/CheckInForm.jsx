import { useState, useEffect } from "react";
import { useCheckIn } from "../hooks/useCheckIn";
import { getAllReservationsWithDetails } from "../../../api/reservas.api";

export default function CheckInForm() {
    const [reservas, setReservas] = useState([]);

    const [form, setForm] = useState({
        reservaId: "",
        fechaCheckIn: "",
        observaciones: "",
    });

    const { loading, error, result, submit } = useCheckIn();

    useEffect(() => {
        getAllReservationsWithDetails().then((res) => {
            // solo reservas pendientes
            const disponibles = res.data.filter(
                (r) => r.estadoReserva === "Pendiente"
            );
            setReservas(disponibles);
        });
    }, []);

    const handleChange = (e) =>
        setForm({ ...form, [e.target.name]: e.target.value });

    const handleSubmit = (e) => {
        e.preventDefault();

        submit({
            reservaId: form.reservaId,
            fechaCheckIn:
                form.fechaCheckIn || new Date().toISOString(),
            observaciones: form.observaciones,
        });
    };

    return (
        <div className="max-w-2xl mx-auto bg-[#1A2E44] p-8 rounded-xl shadow-lg">
            <h1 className="text-2xl font-bold mb-6">Registrar Check-In</h1>

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
                    name="fechaCheckIn"
                    value={form.fechaCheckIn}
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
                    {loading ? "Registrando..." : "Registrar Check-In"}
                </button>

                {result && (
                    <p className="mt-3 text-green-400 font-semibold">
                        ✔ Check-In registrado correctamente
                    </p>
                )}
                {error && (
                    <p className="mt-3 text-red-400 font-semibold">❌ {error}</p>
                )}
            </form>
        </div>
    );
}