import { useNavigate } from "react-router-dom";
import LayoutDashboard from "../components/LayoutDashboard";
import AgendaCalendar from "../components/AgendaCalendar";
import { useEffect, useState } from "react";
import { getRooms } from "../../../api/habitaciones.api";
import { getAllReservations } from "../../../api/reservas.api";
import { getReporteIngresos } from "../../../api/reportes.api";

export default function DashboardPage() {
  const navigate = useNavigate();
  const [ocupacion, setOcupacion] = useState(0);
  const [habitacionesDisponibles, setHabitacionesDisponibles] = useState(0);
  const [ingresosHoy, setIngresosHoy] = useState(0);
  const [ocupadasHoy, setOcupadasHoy] = useState(0);
  const [totalHabitacionesState, setTotalHabitacionesState] = useState(0);

  useEffect(() => {
    cargarKPIs();
  }, []);

  const cargarKPIs = async () => {
    try {
      // HABITACIONES
      const roomsRes = await getRooms();
      console.log("Habitaciones API:", roomsRes);
      const habitaciones = roomsRes.data.data; // correcto
      const totalHabitaciones = habitaciones.length;
      setTotalHabitacionesState(totalHabitaciones);

      // RESERVAS
      const reservasRes = await getAllReservations();
      console.log("Reservas API:", reservasRes);

      // En tu backend, reservas ya es un array directo
      const reservas = reservasRes.data;

      const hoy = new Date();

      const reservasActivas = reservas.filter((r) => {
        const inicio = new Date(r.fechaInicio);
        const fin = new Date(r.fechaFin);

        // estados válidos de OCUPACIÓN reales de tu backend
        const estadoValido =
          r.estadoReserva === "Activa" || r.estadoReserva === "Confirmada";

        return estadoValido && inicio <= hoy && hoy <= fin;
      });

      const ocupadas = reservasActivas.length;
      setOcupadasHoy(ocupadas);

      // PORCENTAJE
      const porcentaje =
        totalHabitaciones > 0
          ? ((ocupadas / totalHabitaciones) * 100).toFixed(0)
          : 0;

      setOcupacion(porcentaje);
      setHabitacionesDisponibles(totalHabitaciones - ocupadas);

      // INGRESOS DEL DÍA
      const fechaBase = hoy.toISOString().split("T")[0];

      const desde = `${fechaBase}T00:00:00`;
      const hasta = `${fechaBase}T23:59:59`;

      const ingresosRes = await getReporteIngresos(desde, hasta);

      console.log("Ingresos API:", ingresosRes);

      setIngresosHoy(ingresosRes.data.ingresos || 0);
    } catch (error) {
      console.error("Error cargando KPIs:", error);
    }
  };

  return (
    <LayoutDashboard>
      {/* KPIs */}
      <section className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-10">
        {/* Ocupación Actual */}
        <div className="bg-[#1A2E44] p-6 rounded-xl shadow-lg">
          <h3 className="text-xl font-semibold">Ocupación Actual</h3>
          <p className="text-4xl font-bold text-[#FF9900] mt-2">{ocupacion}%</p>
          <p className="text-sm text-gray-300">
            {ocupadasHoy} ocupada(s) de {totalHabitacionesState} habitaciones
          </p>
        </div>

        {/* Ingresos del día */}
        <div className="bg-[#1A2E44] p-6 rounded-xl shadow-lg">
          <h3 className="text-xl font-semibold">Ingresos del Día</h3>
          <p className="text-4xl font-bold text-[#FF9900] mt-2">
            $ {ingresosHoy}
          </p>
          <p className="text-sm text-gray-300">Total facturado hoy</p>
        </div>

        {/* Habitaciones disponibles */}
        <div className="bg-[#1A2E44] p-6 rounded-xl shadow-lg">
          <h3 className="text-xl font-semibold">Disponibles Ahora</h3>
          <p className="text-4xl font-bold text-[#FF9900] mt-2">
            {habitacionesDisponibles}
          </p>
          <p className="text-sm text-gray-300">Habitaciones listas para usar</p>
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
              Check-In / Check-Out
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

          <button
            onClick={() => navigate("/facturacion")}
            className="bg-[#1A2E44] hover:bg-[#243b56] p-6 rounded-xl shadow-lg text-center"
          >
            <p className="text-lg font-semibold text-[#FF9900]">Facturacion</p>
          </button>

          <button
            onClick={() => navigate("/usuarios")}
            className="bg-[#1A2E44] hover:bg-[#243b56] p-6 rounded-xl shadow-lg text-center"
          >
            <p className="text-lg font-semibold text-[#FF9900]">Equipo</p>
          </button>

          <button
            onClick={() => navigate("/roles")}
            className="bg-[#1A2E44] hover:bg-[#243b56] p-6 rounded-xl shadow-lg text-center"
          >
            <p className="text-lg font-semibold text-[#FF9900]">Roles</p>
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
