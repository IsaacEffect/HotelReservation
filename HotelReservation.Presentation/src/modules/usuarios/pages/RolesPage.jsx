import { useEffect, useState } from "react";
import { getRoles } from "../../../api/roles.api";
import { useNavigate } from "react-router-dom";
import LayoutDashboard from "../../usuarios/components/LayoutDashboard";

export default function RolesPage() {
  const [roles, setRoles] = useState([]);
  const [search, setSearch] = useState(""); // buscador
  const navigate = useNavigate();

  useEffect(() => {
    getRoles().then((res) => {
      setRoles(res.data.data);
    });
  }, []);

  // filtrar roles por nombre
  const filteredRoles = roles.filter((r) =>
    r.nombreRol.toLowerCase().includes(search.toLowerCase())
  );

  return (
    <LayoutDashboard>
      <div className="mb-6">
        <div className="flex justify-between items-center mb-6">
          <h1 className="text-2xl font-bold">Roles</h1>

          <button
            onClick={() => navigate("/roles/nuevo")}
            className="bg-[#FF9900] hover:bg-[#D88000] px-4 py-2 rounded text-white font-semibold"
          >
            Nuevo Rol
          </button>
        </div>

        {/* buscador */}
        <input
          type="text"
          placeholder="Buscar rol..."
          value={search}
          onChange={(e) => setSearch(e.target.value)}
          className="w-full p-3 mb-4 rounded bg-[#0F1A2B] text-white border border-[#243b56]"
        />

        {/* tabla */}
        <div className="overflow-x-auto">
          <table className="min-w-full bg-[#1A2E44] text-white rounded-lg overflow-hidden">
            <thead className="bg-[#0F1A2B]">
              <tr>
                <th className="p-3 text-left">Nombre del Rol</th>
              </tr>
            </thead>

            <tbody>
              {filteredRoles.map((role) => (
                <tr key={role.rolId} className="border-b border-[#243b56]">
                  <td className="p-3">{role.nombreRol}</td>
                </tr>
              ))}
            </tbody>
          </table>

          {filteredRoles.length === 0 && (
            <p className="text-gray-400 text-center mt-4">
              No se encontraron roles con ese nombre.
            </p>
          )}
        </div>
      </div>
    </LayoutDashboard>
  );
}
