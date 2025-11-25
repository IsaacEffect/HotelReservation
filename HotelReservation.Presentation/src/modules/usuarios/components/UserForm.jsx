import { useEffect, useState } from "react";
import { getRoles } from "../../../api/roles.api";
import { insertUser, getUserById, updateUser } from "../../../api/users.api";
import { useNavigate, useParams } from "react-router-dom";
import LayoutDashboard from "../components/LayoutDashboard";

export default function UserForm() {
  const [roles, setRoles] = useState([]);
  const [user, setUser] = useState({
    nombre: "",
    apellido: "",
    correo: "",
    contrasena: "",
    rolId: "",
  });

  const navigate = useNavigate();
  const { id } = useParams();
  const editing = !!id;

  useEffect(() => {
    getRoles().then((r) => setRoles(r.data.data));

    if (editing) {
      getUserById(id).then((res) => {
        const u = res.data.data;
        setUser({
          nombre: u.nombre,
          apellido: u.apellido,
          correo: u.correo,
          contrasena: "",
          rolId: u.rol.rolId,
        });
      });
    }
  }, [id, editing]);

  const handleChange = (e) => {
    setUser({
      ...user,
      [e.target.name]: e.target.value,
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (editing) {
      await updateUser(id, user);
    } else {
      await insertUser(user);
    }

    navigate("/usuarios");
  };

  return (
    <LayoutDashboard>
      <div className="max-w-lg mx-auto bg-[#1A2E44] p-8 rounded-xl shadow-lg">
        <h1 className="text-2xl font-bold mb-6">
          {editing ? "Editar Usuario" : "Nuevo Usuario"}
        </h1>

        <form onSubmit={handleSubmit} className="flex flex-col gap-4">
          <input
            type="text"
            name="nombre"
            placeholder="Nombre"
            value={user.nombre}
            onChange={handleChange}
            className="bg-[#0F1A2B] p-3 rounded text-white"
            required
          />

          <input
            type="text"
            name="apellido"
            placeholder="Apellido"
            value={user.apellido}
            onChange={handleChange}
            className="bg-[#0F1A2B] p-3 rounded text-white"
            required
          />

          <input
            type="email"
            name="correo"
            placeholder="Correo"
            value={user.correo}
            onChange={handleChange}
            className="bg-[#0F1A2B] p-3 rounded text-white"
            required
          />

          {!editing && (
            <input
              type="password"
              name="contrasena"
              placeholder="Contraseña"
              value={user.contrasena}
              onChange={handleChange}
              className="bg-[#0F1A2B] p-3 rounded text-white"
              required
            />
          )}

          {/* ROLES */}
          <select
            name="rolId"
            value={user.rolId}
            onChange={handleChange}
            className="bg-[#0F1A2B] p-3 rounded text-white"
            required
          >
            <option value="">Seleccione un rol</option>
            {roles.map((r) => (
              <option key={r.rolId} value={r.rolId}>
                {r.nombreRol}
              </option>
            ))}
          </select>

          <button className="bg-[#FF9900] hover:bg-[#D88000] py-3 rounded text-white font-semibold">
            Guardar
          </button>
        </form>
      </div>
    </LayoutDashboard>
  );
}
