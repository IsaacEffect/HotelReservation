import { useEffect, useState } from "react";
import {
  getAllReservationsWithDetails,
  cancelReservation,
} from "../../../api/reservas.api";
import { useNavigate } from "react-router-dom";
import LayoutDashboard from "../../usuarios/components/LayoutDashboard";

export default function ReservasPage() {
  const [reservas, setReservas] = useState([]);
  const [search, setSearch] = useState(""); // 🔍 Nuevo estado de búsqueda
  const navigate = useNavigate();

  const loadReservas = async () => {
    const res = await getAllReservationsWithDetails();
    setReservas(res.data);
  };

  useEffect(() => {
    loadReservas();
  }, []);

  const handleCancel = async (id) => {
    if (!window.confirm("¿Deseas cancelar esta reserva?")) return;
    await cancelReservation(id);
    loadReservas();
  };

  // 🔍 Filtrar resultados
  const filteredReservas = reservas.filter((r) => {
    const text =
      `${r.cliente} ${r.correoCliente} ${r.numeroHabitacion} ${r.categoria} ${r.estadoReserva}`.toLowerCase();
    return text.includes(search.toLowerCase());
  });

  return (
    <LayoutDashboard>
      <div>
        {/* Header */}
        <div className="flex justify-between items-center mb-6">
          <h1 className="text-2xl font-bold">Reservas</h1>

          <button
            onClick={() => navigate("/reservas/nueva")}
            className="bg-[#FF9900] hover:bg-[#D88000] px-4 py-2 rounded text-white font-semibold"
          >
            Nueva Reserva
          </button>
        </div>

        {/* 🔍 Buscador */}
        <div className="mb-4">
          <input
            type="text"
            placeholder="Buscar por cliente, estado o habitación..."
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            className="w-full p-3 rounded-lg bg-[#1A2E44] text-white border 
                       border-[#FF9900]/40 focus:border-[#FF9900] focus:outline-none"
          />
        </div>

        {/* Tabla */}
        <div className="overflow-x-auto">
          <table className="min-w-full bg-[#1A2E44] text-white rounded-lg overflow-hidden">
            <thead className="bg-[#0F1A2B]">
              <tr>
                <th className="p-3">Cliente</th>
                <th className="p-3">Hab.</th>
                <th className="p-3">Categoría</th>
                <th className="p-3">Estado</th>
                <th className="p-3">Fechas</th>
                <th className="p-3">Total</th>
                <th className="p-3">Acciones</th>
              </tr>
            </thead>

            <tbody>
              {filteredReservas.map((r) => (
                <tr key={r.reservaId} className="border-b border-[#243b56]">
                  <td className="p-3">
                    {r.cliente}
                    <br />
                    <span className="text-sm text-gray-300">
                      {r.correoCliente}
                    </span>
                  </td>

                  <td className="p-3">{r.numeroHabitacion}</td>
                  <td className="p-3">{r.categoria}</td>

                  <td className="p-3">{r.estadoReserva}</td>

                  <td className="p-3">
                    {new Date(r.fechaInicio).toLocaleDateString()} -{" "}
                    {new Date(r.fechaFin).toLocaleDateString()}
                  </td>

                  <td className="p-3 font-bold text-[#FF9900]">${r.total}</td>

                  <td className="p-3 flex gap-2">
                    {/* Detalles */}
                    <button
                      className="bg-blue-600 hover:bg-blue-700 px-3 py-1 rounded"
                      onClick={() =>
                        navigate(`/reservas/detalle/${r.reservaId}`)
                      }
                    >
                      Detalle
                    </button>

                    {/* Cancelar */}
                    {r.estadoReserva === "Pendiente" && (
                      <button
                        className="bg-red-600 hover:bg-red-700 px-3 py-1 rounded"
                        onClick={() => handleCancel(r.reservaId)}
                      >
                        Cancelar
                      </button>
                    )}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>

          {filteredReservas.length === 0 && (
            <p className="text-center text-gray-300 mt-6">
              No hay resultados para la búsqueda.
            </p>
          )}
        </div>
      </div>
    </LayoutDashboard>
  );
}
