import { useState } from "react";
import { changePassword } from "../../../api/users.api";
import { useNavigate, useParams } from "react-router-dom";
import LayoutDashboard from "../components/LayoutDashboard";

export default function ChangePassword() {
  const navigate = useNavigate();
  const { id } = useParams();

  const [form, setForm] = useState({
    idUsuario: id,
    contrasenaActual: "",
    nuevaContrasena: "",
  });

  const handleSubmit = async (e) => {
    e.preventDefault();
    await changePassword(form);
    navigate("/usuarios");
  };

  return (
    <LayoutDashboard>
      <div className="max-w-lg mx-auto bg-[#1A2E44] p-8 rounded-xl shadow-lg">
        <h1 className="text-2xl font-bold mb-6">Cambiar Contraseña</h1>

        <form onSubmit={handleSubmit} className="flex flex-col gap-4">
          <input
            type="password"
            name="contrasenaActual"
            placeholder="Contraseña actual"
            className="bg-[#0F1A2B] p-3 rounded text-white"
            onChange={(e) =>
              setForm({ ...form, contrasenaActual: e.target.value })
            }
          />

          <input
            type="password"
            name="nuevaContrasena"
            placeholder="Nueva contraseña"
            className="bg-[#0F1A2B] p-3 rounded text-white"
            onChange={(e) =>
              setForm({ ...form, nuevaContrasena: e.target.value })
            }
          />

          <button className="bg-[#FF9900] hover:bg-[#D88000] py-3 rounded text-white font-semibold">
            Guardar
          </button>
        </form>
      </div>
    </LayoutDashboard>
  );
}
