import { useEffect, useState } from "react";
import { getRooms, deleteHabitacion } from "../../../api/habitaciones.api";
import { getCategorias } from "../../../api/categorias.api";
import { useNavigate } from "react-router-dom";
import LayoutDashboard from "../../usuarios/components/LayoutDashboard";

export default function HabitacionesPage() {
  const [habitaciones, setHabitaciones] = useState([]);
  const [filter, setFilter] = useState("");
  const navigate = useNavigate();

  const loadHabitaciones = async () => {
    const resHab = await getRooms();
    const resCat = await getCategorias();

    const habitaciones = resHab.data.data;
    const categorias = resCat.data.data;

    // uniendo categoría con habitación
    const habitacionesConCategoria = habitaciones.map((h) => {
      const categoria = categorias.find((c) => c.id === h.categoriaId) || null;

      return {
        ...h,
        categoria,
        detalle: categoria?.descripcion || "N/A",
        precio: categoria?.precioPorNoche || 0,
        piso: "1",
      };
    });

    setHabitaciones(habitacionesConCategoria);
  };

  useEffect(() => {
    loadHabitaciones();
  }, []);

  const filteredHabitaciones = habitaciones.filter(
    (h) =>
      h.numero.toLowerCase().includes(filter.toLowerCase()) ||
      (h.detalle || "").toLowerCase().includes(filter.toLowerCase())
  );

  const handleDelete = async (id) => {
    if (!window.confirm("¿Eliminar esta habitación?")) return;
    await deleteHabitacion(id);
    loadHabitaciones();
  };

  return (
    <LayoutDashboard>
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold">Habitaciones</h1>

        <div className="flex gap-3">
          <button
            onClick={() => navigate("/categorias")}
            className="bg-[#0ea5e9] hover:bg-[#0284c7] px-4 py-2 rounded text-white font-semibold"
          >
            Ver Categorías
          </button>

          <button
            onClick={() => navigate("/habitaciones/nueva")}
            className="bg-[#FF9900] hover:bg-[#D88000] px-4 py-2 rounded text-white font-semibold"
          >
            Nueva Habitación
          </button>
        </div>
      </div>

      {/* BUSCADOR */}
      <input
        type="text"
        placeholder="Buscar por número o detalle..."
        className="w-full p-3 mb-4 rounded bg-[#1A2E44] text-white"
        value={filter}
        onChange={(e) => setFilter(e.target.value)}
      />

      {/* TABLA */}
      <div className="overflow-x-auto">
        <table className="min-w-full bg-[#1A2E44] text-white rounded-lg overflow-hidden">
          <thead className="bg-[#0F1A2B]">
            <tr>
              <th className="p-3 text-left">Número</th>
              <th className="p-3 text-left">Detalle</th>
              <th className="p-3 text-left">Precio</th>
              <th className="p-3 text-left">Estado</th>
              <th className="p-3 text-left">Categoría</th>
              <th className="p-3 text-left">Piso</th>
              <th className="p-3 text-left">Acciones</th>
            </tr>
          </thead>

          <tbody>
            {filteredHabitaciones.map((h) => (
              <tr key={h.id} className="border-b border-[#243b56]">
                <td className="p-3">{h.numero}</td>
                <td className="p-3">{h.detalle}</td>
                <td className="p-3">${h.precio}</td>
                <td className="p-3">
                  <span
                    className={`px-2 py-1 rounded text-xs ${
                      h.estado === "Disponible"
                        ? "bg-green-600"
                        : h.estado === "Ocupada"
                        ? "bg-red-600"
                        : "bg-yellow-600"
                    }`}
                  >
                    {h.estado}
                  </span>
                </td>
                <td className="p-3">{h.categoria?.nombreCategoria || "N/A"}</td>
                <td className="p-3">{h.piso}</td>

                <td className="p-3 flex gap-2">
                  <button
                    className="bg-blue-600 hover:bg-blue-700 px-3 py-1 rounded text-white"
                    onClick={() => navigate(`/habitaciones/editar/${h.id}`)}
                  >
                    Editar
                  </button>

                  <button
                    className="bg-red-600 hover:bg-red-700 px-3 py-1 rounded text-white"
                    onClick={() => handleDelete(h.id)}
                  >
                    Eliminar
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>

        {filteredHabitaciones.length === 0 && (
          <p className="text-center text-gray-300 mt-6">
            No hay habitaciones registradas.
          </p>
        )}
      </div>
    </LayoutDashboard>
  );
}
