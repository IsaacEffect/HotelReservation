import { useState } from "react";
import { useAuth } from "../../../app/context/useAuth";

export default function HeaderDashboard() {
  const { user, logout } = useAuth();
  const [openMenu, setOpenMenu] = useState(false);

  return (
    <header className="w-full bg-[#1A2E44] text-white px-6 py-4 shadow-md flex justify-end items-center relative">
      {/* CONTENEDOR DEL PERFIL */}
      <div
        className="flex items-center gap-3 cursor-pointer"
        onClick={() => setOpenMenu(!openMenu)}
      >
        {/* Ícono circular */}
        <div className="w-10 h-10 bg-[#FF9900] text-black font-bold rounded-full flex items-center justify-center">
          {user?.nombre ? user.nombre[0] : "U"}
        </div>

        {/* Nombre */}
        <p className="font-medium">{user?.nombre || "Usuario"}</p>
      </div>

      {/* MENÚ DROPDOWN */}
      {openMenu && (
        <div className="absolute top-16 right-6 bg-[#1A2E44] border border-[#FF9900]/40 rounded-lg shadow-lg w-44 p-2">
          <button
            onClick={logout}
            className="w-full text-left px-3 py-2 rounded hover:bg-[#243b56] transition"
          >
            Cerrar sesión
          </button>
        </div>
      )}
    </header>
  );
}
