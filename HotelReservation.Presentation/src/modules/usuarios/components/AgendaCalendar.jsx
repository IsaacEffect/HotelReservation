import FullCalendar from "@fullcalendar/react";
import dayGridPlugin from "@fullcalendar/daygrid";
import timeGridPlugin from "@fullcalendar/timegrid";
import interactionPlugin from "@fullcalendar/interaction";

export default function AgendaCalendar() {

  const today = new Date().toISOString().split("T")[0];

  // Eventos falsos estilo hotel
  const events = [
    {
      title: "Check-in - Juan Pérez",
      start: `${today}T10:00:00`,
      end: `${today}T11:00:00`,
      color: "#FF9900", // Naranja cobre
    },
    {
      title: "Check-out - María López",
      start: `${today}T13:00:00`,
      end: `${today}T14:00:00`,
      color: "#1E90FF", // Azul suave
    },
    {
      title: "Estadía Activa - Hab 204",
      start: `${today}T00:00:00`,
      end: `${today}T23:59:00`,
      color: "#00C851", // Verde
    },
    {
      title: "Mantenimiento - Hab 310",
      start: `${today}T15:00:00`,
      end: `${today}T17:30:00`,
      color: "#D88000", // Dorado oscuro
    },
  ];

  return (
    <div className="bg-[#1A2E44] p-6 rounded-xl shadow-lg">
      <FullCalendar
        plugins={[dayGridPlugin, timeGridPlugin, interactionPlugin]}
        initialView="timeGridWeek"
        height="650px"
        titleFormat={{ year: "numeric", month: "long" }}
        headerToolbar={{
          left: "prev,next",
          center: "title",
          right: "",
        }}
        slotMinTime="06:00:00"
        slotMaxTime="22:00:00"
        allDaySlot={false}
        events={events}
        slotLabelClassNames="text-gray-200"
        dayHeaderClassNames="text-white"
        eventClassNames="text-sm font-semibold"
      />
    </div>
  );
}
