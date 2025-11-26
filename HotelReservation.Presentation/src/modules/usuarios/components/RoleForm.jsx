import { useState } from "react";
import { insertRole } from "../../../api/roles.api";
import { useNavigate } from "react-router-dom";
import LayoutDashboard from "../../usuarios/components/LayoutDashboard";

export default function RoleForm() {
  const [nombreRol, setNombreRol] = useState("");
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (nombreRol.trim().length < 3) {
      alert("El nombre del rol debe tener al menos 3 caracteres.");
      return;
    }

    await insertRole({ nombreRol });
    navigate("/roles");
  };

  return (
    <LayoutDashboard>
      <div className="max-w-md mx-auto bg-[#1A2E44] p-8 rounded-xl shadow-lg">
        <h1 className="text-2xl font-bold mb-6">Nuevo Rol</h1>

        <form onSubmit={handleSubmit} className="flex flex-col gap-4">
          <input
            type="text"
            placeholder="Nombre del Rol"
            value={nombreRol}
            onChange={(e) => setNombreRol(e.target.value)}
            className="bg-[#0F1A2B] p-3 rounded text-white"
            required
          />

          <button className="bg-[#FF9900] hover:bg-[#D88000] py-3 rounded text-white font-semibold">
            Guardar
          </button>
        </form>
      </div>
    </LayoutDashboard>
  );
}
