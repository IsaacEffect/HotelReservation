import HeaderDashboard from "./HeaderDashboard";

export default function LayoutDashboard({ children }) {
  return (
    <div className="flex min-h-screen bg-[#0F1A2B] text-white">
      {/* SIDEBAR */}
      <aside className="w-64 bg-[#1A2E44] p-6 flex flex-col gap-6 shadow-lg">
        <h1 className="text-2xl font-bold text-[#FF9900]">RoyalKey</h1>

        <nav className="flex flex-col gap-3 text-lg">
          <a href="/" className="hover:text-[#FF9900]">
            Dashboard
          </a>
          <a href="/reservas" className="hover:text-[#FF9900]">
            Reservas
          </a>
          <a href="/check" className="hover:text-[#FF9900]">
            Check-in / Check-out
          </a>
          <a href="/clientes" className="hover:text-[#FF9900]">
            Clientes
          </a>
          <a href="/habitaciones" className="hover:text-[#FF9900]">
            Habitaciones
          </a>
          <a href="/reportes" className="hover:text-[#FF9900]">
            Reportes
          </a>
        </nav>
      </aside>

      {/* CONTENIDO */}
      <main className="flex-1 flex flex-col">
        {/* HEADER */}
        <HeaderDashboard />

        {/* CONTENIDO DINÁMICO */}
        <div className="p-8 flex-1">{children}</div>
      </main>
    </div>
  );
}
