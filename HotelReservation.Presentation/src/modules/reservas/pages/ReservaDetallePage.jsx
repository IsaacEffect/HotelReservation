import { useEffect, useState, useCallback } from "react";
import { useNavigate, useParams } from "react-router-dom";
import {
  getReservationDetailsById,
  cancelReservation,
} from "../../../api/reservas.api";
import LayoutDashboard from "../../usuarios/components/LayoutDashboard";

export default function ReservaDetallePage() {
  const { id } = useParams();
  const [reserva, setReserva] = useState(null);
  const navigate = useNavigate();

  const loadReserva = useCallback(async () => {
    const res = await getReservationDetailsById(id);
    setReserva(res.data);
  }, [id]);

  useEffect(() => {
    loadReserva();
  }, [loadReserva]);

  const handleCancel = async () => {
    if (!window.confirm("¿Deseas cancelar esta reserva?")) return;
    await cancelReservation(id);
    await loadReserva();
  };

  if (!reserva)
    return (
      <LayoutDashboard>
        <p className="text-white">Cargando...</p>
      </LayoutDashboard>
    );

  return (
    <LayoutDashboard>
      <div className="max-w-3xl mx-auto bg-[#1A2E44] p-8 rounded-xl shadow-lg">
        <h1 className="text-2xl font-bold mb-6">Detalle de Reserva</h1>

        <div className="space-y-3">
          <p>
            <strong>Cliente:</strong> {reserva.cliente} ({reserva.correoCliente}
            )
          </p>
          <p>
            <strong>Habitación:</strong> {reserva.numeroHabitacion} —{" "}
            {reserva.categoria}
          </p>
          <p>
            <strong>Fecha Reserva:</strong>{" "}
            {new Date(reserva.fechaReserva).toLocaleDateString()}
          </p>
          <p>
            <strong>Fechas:</strong>{" "}
            {new Date(reserva.fechaInicio).toLocaleDateString()} -{" "}
            {new Date(reserva.fechaFin).toLocaleDateString()}
          </p>
          <p>
            <strong>Precio por noche:</strong> ${reserva.precioPorNoche}
          </p>
          <p>
            <strong>Estado:</strong>{" "}
            <span className="text-[#FF9900]">{reserva.estadoReserva}</span>
          </p>
          <p>
            <strong>Registrado por:</strong> {reserva.usuarioRegistro}
          </p>

          <p className="text-xl font-bold text-[#FF9900]">
            Total: ${reserva.total}
          </p>
        </div>

        {/* Acciones */}
        <div className="flex gap-4 mt-8">
          <button
            className="bg-gray-600 hover:bg-gray-700 px-4 py-2 rounded text-white"
            onClick={() => navigate("/reservas")}
          >
            Volver
          </button>

          {reserva.estadoReserva === "Pendiente" && (
            <button
              className="bg-red-600 hover:bg-red-700 px-4 py-2 rounded text-white"
              onClick={handleCancel}
            >
              Cancelar Reserva
            </button>
          )}
        </div>
      </div>
    </LayoutDashboard>
  );
}
