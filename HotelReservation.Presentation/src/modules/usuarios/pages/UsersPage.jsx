import { useEffect, useState } from "react";
import { getUsers, deleteUser } from "../../../api/users.api";
import { useNavigate } from "react-router-dom";
import LayoutDashboard from "../components/LayoutDashboard";

export default function UsersPage() {
  const [users, setUsers] = useState([]);
  const [filter, setFilter] = useState("");
  const navigate = useNavigate();

  const loadUsers = () => {
    getUsers().then((res) => setUsers(res.data.data));
  };

  useEffect(() => {
    loadUsers();
  }, []);

  const filteredUsers = users.filter(
    (u) =>
      u.nombre.toLowerCase().includes(filter.toLowerCase()) ||
      u.apellido.toLowerCase().includes(filter.toLowerCase()) ||
      u.correo.toLowerCase().includes(filter.toLowerCase()) ||
      u.rol.nombreRol.toLowerCase().includes(filter.toLowerCase())
  );

  const handleDelete = async (id) => {
    if (!window.confirm("¿Eliminar este usuario?")) return;
    await deleteUser(id);
    loadUsers();
  };

  return (
    <LayoutDashboard>
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold">Usuarios</h1>

        <button
          onClick={() => navigate("/usuarios/nuevo")}
          className="bg-[#FF9900] hover:bg-[#D88000] px-4 py-2 rounded text-white font-semibold"
        >
          Nuevo Usuario
        </button>
      </div>

      {/* BUSCADOR */}
      <input
        type="text"
        placeholder="Buscar por nombre, correo o rol..."
        className="w-full p-3 mb-4 rounded bg-[#1A2E44] text-white"
        value={filter}
        onChange={(e) => setFilter(e.target.value)}
      />

      {/* TABLA */}
      <div className="overflow-x-auto">
        <table className="min-w-full bg-[#1A2E44] text-white rounded-lg overflow-hidden">
          <thead className="bg-[#0F1A2B]">
            <tr>
              <th className="p-3 text-left">Nombre</th>
              <th className="p-3 text-left">Correo</th>
              <th className="p-3 text-left">Rol</th>
              <th className="p-3 text-left">Acciones</th>
            </tr>
          </thead>

          <tbody>
            {filteredUsers.map((u) => (
              <tr key={u.idUsuario} className="border-b border-[#243b56]">
                <td className="p-3">
                  {u.nombre} {u.apellido}
                </td>
                <td className="p-3">{u.correo}</td>
                <td className="p-3">{u.rol.nombreRol}</td>

                <td className="p-3 flex gap-2">
                  <button
                    className="bg-blue-600 hover:bg-blue-700 px-3 py-1 rounded text-white"
                    onClick={() => navigate(`/usuarios/editar/${u.idUsuario}`)}
                  >
                    Editar
                  </button>

                  <button
                    className="bg-yellow-600 hover:bg-yellow-700 px-3 py-1 rounded text-white"
                    onClick={() =>
                      navigate(`/usuarios/cambiar-pass/${u.idUsuario}`)
                    }
                  >
                    Cambiar Pass
                  </button>

                  <button
                    className="bg-red-600 hover:bg-red-700 px-3 py-1 rounded text-white"
                    onClick={() => handleDelete(u.idUsuario)}
                  >
                    Eliminar
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>

        {filteredUsers.length === 0 && (
          <p className="text-center text-gray-300 mt-6">No hay usuarios.</p>
        )}
      </div>
    </LayoutDashboard>
  );
}
