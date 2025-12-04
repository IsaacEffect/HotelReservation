import { useEffect, useState } from "react";
import FullCalendar from "@fullcalendar/react";
import dayGridPlugin from "@fullcalendar/daygrid";
import timeGridPlugin from "@fullcalendar/timegrid";
import interactionPlugin from "@fullcalendar/interaction";

import { getAllReservations } from "../../../api/reservas.api";
import { getRooms } from "../../../api/habitaciones.api";
import { getClientById } from "../../../api/clients.api";

export default function AgendaCalendar() {
  const [events, setEvents] = useState([]);

  useEffect(() => {
    cargarEventos();
  }, []);

  // ----------------------------
  // 🎨 Evento con diseño Premium
  // ----------------------------
  const renderEventContent = (eventInfo) => {
    const title = eventInfo.event.title;

    let icon = "📌";
    if (title.toLowerCase().includes("check-in")) icon = "🟧";
    if (title.toLowerCase().includes("check-out")) icon = "🔵";
    if (title.toLowerCase().includes("estadía")) icon = "🟩";

    return (
      <div className="fc-event-content-custom">
        <span className="fc-event-icon">{icon}</span>
        <span className="fc-event-title">{title}</span>
      </div>
    );
  };

  const cargarEventos = async () => {
    try {
      const reservasRes = await getAllReservations();
      const reservas = reservasRes.data;

      const roomsRes = await getRooms();
      const rooms = roomsRes.data.data;

      const hoy = new Date().toISOString().split("T")[0];

      const eventosPromesas = reservas.map(async (r) => {
        const inicio = r.fechaInicio.split("T")[0];
        const fin = r.fechaFin.split("T")[0];

        // obtener habitación
        const hab = rooms.find((h) => h.id === r.habitacionId);
        const habNum = hab?.numero ?? "S/N";

        // obtener cliente
        const clienteRes = await getClientById(r.clienteId);
        const cliente = clienteRes.data.data;
        const nombreCliente = `${cliente.nombre} ${cliente.apellido}`;

        let eventos = [];

        // CHECK-IN HOY
        if (
          inicio === hoy &&
          (r.estadoReserva === "Activa" || r.estadoReserva === "Confirmada")
        ) {
          eventos.push({
            title: `Check-in • Hab ${habNum} • ${nombreCliente}`,
            start: `${inicio}T14:00:00`,
            classNames: ["event-checkin"],
          });
        }

        // CHECK-OUT HOY
        if (
          fin === hoy &&
          (r.estadoReserva === "Activa" || r.estadoReserva === "Confirmada")
        ) {
          eventos.push({
            title: `Check-out • Hab ${habNum} • ${nombreCliente}`,
            start: `${fin}T12:00:00`,
            classNames: ["event-checkout"],
          });
        }

        // ESTADÍA ACTIVA HOY
        const hoyDate = new Date(hoy);
        const startDate = new Date(inicio);
        const endDate = new Date(fin);

        const dentro =
          hoyDate >= startDate &&
          hoyDate <= endDate &&
          (r.estadoReserva === "Activa" || r.estadoReserva === "Confirmada");

        if (dentro) {
          eventos.push({
            title: `Estadía Activa • Hab ${habNum} • ${nombreCliente}`,
            start: `${hoy}T00:00:00`,
            end: `${hoy}T23:59:00`,
            classNames: ["event-stay"],
          });
        }

        return eventos;
      });

      const eventosGenerados = (await Promise.all(eventosPromesas)).flat();
      setEvents(eventosGenerados);
    } catch (error) {
      console.error("Error cargando eventos:", error);
    }
  };

  return (
    <div className="bg-[#1A2E44] p-6 rounded-xl shadow-lg">
      <FullCalendar
        plugins={[dayGridPlugin, timeGridPlugin, interactionPlugin]}
        initialView="timeGridWeek"
        height="650px"
        headerToolbar={{
          left: "prev,next",
          center: "title",
          right: "",
        }}
        slotMinTime="06:00:00"
        slotMaxTime="22:00:00"
        allDaySlot={false}
        events={events}
        eventContent={renderEventContent}
        eventClassNames="custom-event"
        slotLabelClassNames="text-gray-300"
        dayHeaderClassNames="text-[#FF9900] font-bold"
        titleFormat={{ year: "numeric", month: "long" }}
      />
    </div>
  );
}
