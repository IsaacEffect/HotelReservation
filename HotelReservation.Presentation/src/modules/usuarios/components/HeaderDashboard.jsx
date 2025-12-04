import { useState } from "react";
import { useAuth } from "../../../app/context/useAuth";
import { LogOut, ChevronDown } from "lucide-react";

export default function HeaderDashboard() {
  const { user, logout } = useAuth();
  const [openMenu, setOpenMenu] = useState(false);

  const initials = user?.nombre?.[0] || "U";

  return (
    <header className="w-full bg-[#162537] text-white px-8 py-4 shadow-md flex justify-end items-center relative border-b border-[#22374E]">
      {/* PERFIL */}
      <button
        onClick={() => setOpenMenu(!openMenu)}
        className="flex items-center gap-3 cursor-pointer group"
      >
        {/* Avatar */}
        <div className="w-10 h-10 bg-[#FF9900] text-black font-bold rounded-full flex items-center justify-center shadow-md group-hover:opacity-90 transition">
          {initials}
        </div>

        {/* Nombre + icono */}
        <div className="flex items-center gap-1">
          <p className="font-semibold tracking-wide text-gray-200 group-hover:text-white transition">
            {user?.nombre || "Usuario"}
          </p>
          <ChevronDown
            size={18}
            className={`transition ${
              openMenu ? "rotate-180 text-[#FF9900]" : "text-gray-300"
            }`}
          />
        </div>
      </button>

      {/* DROPDOWN */}
      {openMenu && (
        <div className="absolute top-16 right-8 bg-[#1A2E44] border border-[#FF9900]/40 rounded-xl shadow-xl w-48 py-2 animate-fadeIn">
          <button
            onClick={logout}
            className="w-full flex items-center gap-3 px-4 py-2 text-left text-gray-200 hover:bg-[#243b56] hover:text-white transition rounded-lg"
          >
            <LogOut size={18} />
            Cerrar sesión
          </button>
        </div>
      )}
    </header>
  );
}
