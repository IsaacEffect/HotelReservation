import { useEffect, useState } from "react";
import { getCategorias, deleteCategoria } from "../../../api/categorias.api";
import { useNavigate } from "react-router-dom";
import LayoutDashboard from "../../usuarios/components/LayoutDashboard";

export default function CategoriasPage() {
  const [categorias, setCategorias] = useState([]);
  const [filter, setFilter] = useState("");
  const navigate = useNavigate();

  const loadCategorias = () => {
    getCategorias().then((res) => setCategorias(res.data.data));
  };

  useEffect(() => {
    loadCategorias();
  }, []);

  const filteredCategorias = categorias.filter(
    (c) =>
      c.nombreCategoria.toLowerCase().includes(filter.toLowerCase()) ||
      c.descripcion.toLowerCase().includes(filter.toLowerCase())
  );

  const handleDelete = async (id) => {
    if (!window.confirm("¿Eliminar esta categoría?")) return;
    await deleteCategoria(id);
    loadCategorias();
  };

  return (
    <LayoutDashboard>
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold">Categorías</h1>

        <button
          onClick={() => navigate("/categorias/nueva")}
          className="bg-[#FF9900] hover:bg-[#D88000] px-4 py-2 rounded text-white font-semibold"
        >
          Nueva Categoría
        </button>
      </div>

      {/* BUSCADOR */}
      <input
        type="text"
        placeholder="Buscar por nombre o detalle..."
        className="w-full p-3 mb-4 rounded bg-[#1A2E44] text-white"
        value={filter}
        onChange={(e) => setFilter(e.target.value)}
      />

      {/* TABLA */}
      <div className="overflow-x-auto">
        <table className="min-w-full bg-[#1A2E44] text-white rounded-lg overflow-hidden">
          <thead className="bg-[#0F1A2B]">
            <tr>
              <th className="p-4 text-left">Nombre</th>
              <th className="p-4 text-left w-[40%]">Detalle</th>
              <th className="p-4 text-left">Capacidad</th>
              <th className="p-4 text-left">Precio</th>
              <th className="p-4 text-left">Acciones</th>
            </tr>
          </thead>

          <tbody>
            {filteredCategorias.map((c) => (
              <tr key={c.id} className="border-b border-[#243b56] align-middle">
                <td className="p-4">{c.nombreCategoria}</td>
                <td className="p-4 w-[40%]">{c.descripcion}</td>
                <td className="p-4">{c.capacidad}</td>
                <td className="p-4">{c.precioPorNoche}</td>

                <td className="p-4 flex gap-3">
                  <button
                    className="bg-blue-600 hover:bg-blue-700 px-3 py-1 rounded text-white"
                    onClick={() => navigate(`/categorias/editar/${c.id}`)}
                  >
                    Editar
                  </button>

                  <button
                    className="bg-red-600 hover:bg-red-700 px-3 py-1 rounded text-white"
                    onClick={() => handleDelete(c.id)}
                  >
                    Eliminar
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>

        {filteredCategorias.length === 0 && (
          <p className="text-center text-gray-300 mt-6">No hay categorías.</p>
        )}
      </div>
    </LayoutDashboard>
  );
}
