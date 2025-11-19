import { useNavigate } from "react-router-dom";
import LayoutDashboard from "../components/LayoutDashboard";
import AgendaCalendar from "../components/AgendaCalendar";

export default function DashboardPage() {
  const navigate = useNavigate();
  return (
    <LayoutDashboard>
      {/* KPIs */}
      <section className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-10">
        {/* Ocupación Actual */}
        <div className="bg-[#1A2E44] p-6 rounded-xl shadow-lg">
          <h3 className="text-xl font-semibold">Ocupación Actual</h3>
          <p className="text-4xl font-bold text-[#FF9900] mt-2">78%</p>
          <p className="text-sm text-gray-300">42 de 54 habitaciones</p>
        </div>

        {/* Ingresos Proyectados */}
        <div className="bg-[#1A2E44] p-6 rounded-xl shadow-lg">
          <h3 className="text-xl font-semibold">Ingresos del Día</h3>
          <p className="text-4xl font-bold text-[#FF9900] mt-2">$ 18,450</p>
          <p className="text-sm text-gray-300">Proyección total</p>
        </div>

        {/* Habitaciones disponibles */}
        <div className="bg-[#1A2E44] p-6 rounded-xl shadow-lg">
          <h3 className="text-xl font-semibold">Disponibles Ahora</h3>
          <p className="text-4xl font-bold text-[#FF9900] mt-2">12</p>
          <p className="text-sm text-gray-300">Habitaciones listas</p>
        </div>
      </section>

      {/* ACCESO RÁPIDO */}
      <section className="mb-10">
        <h2 className="text-2xl font-bold mb-4">Acceso Rápido</h2>

        <div className="grid grid-cols-2 md:grid-cols-5 gap-6">
          <button
            onClick={() => navigate("/reservas")}
            className="bg-[#1A2E44] hover:bg-[#243b56] p-6 rounded-xl shadow-lg text-center"
          >
            <p className="text-lg font-semibold text-[#FF9900]">Reservas</p>
          </button>

          <button
            onClick={() => navigate("/check")}
            className="bg-[#1A2E44] hover:bg-[#243b56] p-6 rounded-xl shadow-lg text-center"
          >
            <p className="text-lg font-semibold text-[#FF9900]">
              Check-in / Check-out
            </p>
          </button>

          <button
            onClick={() => navigate("/clientes")}
            className="bg-[#1A2E44] hover:bg-[#243b56] p-6 rounded-xl shadow-lg text-center"
          >
            <p className="text-lg font-semibold text-[#FF9900]">Clientes</p>
          </button>

          <button
            onClick={() => navigate("/habitaciones")}
            className="bg-[#1A2E44] hover:bg-[#243b56] p-6 rounded-xl shadow-lg text-center"
          >
            <p className="text-lg font-semibold text-[#FF9900]">Habitaciones</p>
          </button>

          <button
            onClick={() => navigate("/reportes")}
            className="bg-[#1A2E44] hover:bg-[#243b56] p-6 rounded-xl shadow-lg text-center"
          >
            <p className="text-lg font-semibold text-[#FF9900]">Reportes</p>
          </button>
        </div>
      </section>

      {/* CALENDARIO */}
      <section>
        <h2 className="text-2xl font-bold mb-4">Llegadas y Salidas de Hoy</h2>

        <div className="bg-[#1A2E44] p-6 rounded-xl shadow-lg">
          <section>
            <h2 className="text-2xl font-bold mb-4">
              Llegadas y Salidas de Hoy
            </h2>
            <AgendaCalendar />
          </section>
        </div>
      </section>
    </LayoutDashboard>
  );
}
