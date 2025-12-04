import { useState } from "react";
import HeaderDashboard from "./HeaderDashboard";
import { useLocation, useNavigate } from "react-router-dom";
import {
  CalendarCheck,
  Users,
  BedDouble,
  FileText,
  CreditCard,
  Shield,
  LogIn,
  ChevronLeft,
  ChevronRight,
} from "lucide-react";

export default function LayoutDashboard({ children }) {
  const [collapsed, setCollapsed] = useState(false);
  const location = useLocation();
  const navigate = useNavigate();

  const menuItems = [
    { name: "Reservas", path: "/reservas", icon: CalendarCheck },
    { name: "Check-In / Check-Out", path: "/check", icon: LogIn },
    { name: "Clientes", path: "/clientes", icon: Users },
    { name: "Habitaciones", path: "/habitaciones", icon: BedDouble },
    { name: "Reportes", path: "/reportes", icon: FileText },
    { name: "Facturación", path: "/facturacion", icon: CreditCard },
    { name: "Equipo", path: "/usuarios", icon: Users },
    { name: "Roles", path: "/roles", icon: Shield },
  ];

  return (
    <div className="flex min-h-screen bg-[#0F1A2B] text-white relative">
      {/* SIDEBAR */}
      <aside
        className={`
          ${collapsed ? "w-20" : "w-64"}
          bg-[#162537] p-6 flex flex-col shadow-xl border-r border-[#22374E]
          transition-all duration-300
        `}
      >
        {/* LOGO */}
        <a
          onClick={() => navigate("/")}
          className="hover:opacity-80 transition cursor-pointer mb-8"
        >
          {!collapsed && (
            <h1 className="text-3xl font-bold tracking-wide text-[#FF9900]">
              RoyalKey
            </h1>
          )}

          {collapsed && (
            <h1 className="text-3xl font-extrabold text-[#FF9900] text-center">
              R
            </h1>
          )}
        </a>

        {/* NAV */}
        <nav className="flex flex-col gap-2 text-base font-medium">
          {menuItems.map((item) => {
            const Icon = item.icon;
            const active = location.pathname.startsWith(item.path);

            return (
              <button
                key={item.path}
                onClick={() => navigate(item.path)}
                className={`
                  flex items-center rounded-xl transition
                  ${
                    active
                      ? "bg-[#FF9900] text-black shadow-md"
                      : "hover:bg-[#20344A] text-gray-300 hover:text-white"
                  }
                  ${
                    collapsed
                      ? "justify-center p-1.5" /* <-- solo icono */
                      : "gap-3 p-[10px] py-3" /* <-- icono + texto */
                  }     
                `}
              >
                <Icon
                  size={20}
                  className={active ? "text-black" : "text-[#FF9900]"}
                />

                {/* Texto solo si no está colapsado */}
                {!collapsed && <span>{item.name}</span>}
              </button>
            );
          })}
        </nav>
      </aside>

      {/* BOTÓN DE COLAPSAR */}
      <button
        onClick={() => setCollapsed(!collapsed)}
        className={`
          absolute top-6 z-50 bg-[#1A2E44] hover:bg-[#243b56]
          text-white p-2 rounded-full shadow-md transition
          ${collapsed ? "left-20" : "left-64"}
        `}
      >
        {collapsed ? <ChevronRight size={20} /> : <ChevronLeft size={20} />}
      </button>

      {/* CONTENIDO */}
      <main className="flex-1 flex flex-col transition-all duration-300">
        <HeaderDashboard />
        <div className="p-8 flex-1">{children}</div>
      </main>
    </div>
  );
}
