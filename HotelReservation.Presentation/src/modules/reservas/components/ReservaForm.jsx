import { useEffect, useState } from "react";
import { getClients } from "../../../api/clients.api";
import { getRooms } from "../../../api/habitaciones.api";
import {
  createReservation,
  checkRoomAvailability,
} from "../../../api/reservas.api";
import { useNavigate } from "react-router-dom";
import LayoutDashboard from "../../usuarios/components/LayoutDashboard";
import { useAuth } from "../../../app/context/useAuth";

export default function ReservaForm() {
  const [clients, setClients] = useState([]);
  const [rooms, setRooms] = useState([]);

  const [form, setForm] = useState({
    clienteId: "",
    habitacionId: "",
    fechaInicio: "",
    fechaFin: "",
  });

  const [selectedRoom, setSelectedRoom] = useState(null);
  const [disponible, setDisponible] = useState(null);

  const navigate = useNavigate();
  const { user } = useAuth();

  // Cargar clientes y habitaciones
  useEffect(() => {
    getClients().then((res) => setClients(res.data.data));
    getRooms().then((res) => setRooms(res.data.data));
  }, []);

  // Al cambiar habitación: cargar detalles
  useEffect(() => {
    const room = rooms.find((r) => r.id === form.habitacionId);
    setSelectedRoom(room || null);
    setDisponible(null); // reset disponibilidad
  }, [form.habitacionId, rooms]);

  // Cálculo de total
  const calcularTotal = () => {
    if (!selectedRoom) return 0;
    const inicio = new Date(form.fechaInicio);
    const fin = new Date(form.fechaFin);
    const noches = (fin - inicio) / (1000 * 60 * 60 * 24);
    return noches > 0 ? noches * selectedRoom.precioPorNoche : 0;
  };

  const handleChange = (e) => {
    setForm({
      ...form,
      [e.target.name]: e.target.value,
    });
    setDisponible(null);
  };

  // Chequear disponibilidad
  const handleCheck = async () => {
    if (!form.habitacionId || !form.fechaInicio || !form.fechaFin) {
      alert("Selecciona habitación y fechas antes de verificar.");
      return;
    }

    const payload = {
      habitacionId: form.habitacionId,
      fechaInicio: form.fechaInicio,
      fechaFin: form.fechaFin,
    };

    const res = await checkRoomAvailability(payload);
    setDisponible(res.data.disponible);
  };

  const handleSubmit = async () => {
    // VALIDACIONES DEL FORM
    if (!form.clienteId) return alert("Debes seleccionar un cliente");
    if (!form.habitacionId) return alert("Debes seleccionar una habitación");
    if (!form.fechaInicio || !form.fechaFin)
      return alert("Selecciona fechas válidas");

    const inicio = new Date(form.fechaInicio);
    const fin = new Date(form.fechaFin);
    const hoy = new Date();

    if (inicio < hoy.setHours(0, 0, 0, 0))
      return alert("La fecha de inicio no puede ser en el pasado");

    if (fin <= inicio)
      return alert("La fecha de fin debe ser mayor a la fecha de inicio");

    if (!user?.idUsuario) return alert("No se pudo identificar el usuario");

    if (disponible !== true) return alert("Primero verifica disponibilidad");

    // OBJETO A ENVIAR
    const newReserva = {
      clienteId: form.clienteId,
      habitacionId: form.habitacionId,
      usuarioId: user.idUsuario,
      fechaInicio: form.fechaInicio,
      fechaFin: form.fechaFin,
    };

    try {
      await createReservation(newReserva);
      alert("¡Reserva creada con éxito! ✅");
      navigate("/reservas");
    } catch (error) {
      console.error(error);
      const msg =
        error.response?.data?.message ||
        "No se pudo crear la reserva, intenta nuevamente";

      alert(`❌ Error: ${msg}`);
    }
  };

  return (
    <LayoutDashboard>
      <div className="max-w-2xl mx-auto bg-[#1A2E44] p-8 rounded-xl shadow-lg">
        <h1 className="text-2xl font-bold mb-6">Nueva Reserva</h1>

        {/* Cliente */}
        <select
          name="clienteId"
          value={form.clienteId}
          onChange={handleChange}
          className="bg-[#0F1A2B] p-3 w-full rounded text-white mb-4"
          required
        >
          <option value="">Seleccione Cliente</option>
          {clients.map((c) => (
            <option key={c.idCliente} value={c.idCliente}>
              {c.nombre} {c.apellido}
            </option>
          ))}
        </select>

        {/* Habitación */}
        <select
          name="habitacionId"
          value={form.habitacionId}
          onChange={handleChange}
          className="bg-[#0F1A2B] p-3 w-full rounded text-white mb-4"
          required
        >
          <option value="">Seleccione Habitación</option>
          {rooms.map((h) => (
            <option key={h.id} value={h.id}>
              {h.numero} — {h.categoriaId}
            </option>
          ))}
        </select>

        {/* Fechas */}
        <div className="flex gap-4">
          <input
            type="date"
            name="fechaInicio"
            value={form.fechaInicio}
            onChange={handleChange}
            className="bg-[#0F1A2B] p-3 rounded text-white w-full"
            required
          />
          <input
            type="date"
            name="fechaFin"
            value={form.fechaFin}
            onChange={handleChange}
            className="bg-[#0F1A2B] p-3 rounded text-white w-full"
            required
          />
        </div>

        {/* Botón Disponibilidad */}
        <button
          className="bg-blue-600 hover:bg-blue-700 py-2 px-4 rounded mt-4"
          onClick={handleCheck}
        >
          Verificar Disponibilidad
        </button>

        {/* Resultado disponibilidad */}
        {disponible !== null && (
          <p
            className={`mt-3 font-bold ${
              disponible ? "text-green-400" : "text-red-400"
            }`}
          >
            {disponible ? "Disponible ✓" : "No Disponible ✗"}
          </p>
        )}

        {/* Info habitación */}
        {selectedRoom && (
          <div className="mt-4 bg-[#0F1A2B] p-4 rounded-lg">
            <p>
              Categoría: <strong>{selectedRoom.categoria}</strong>
            </p>
            <p>
              Precio por noche: <strong>${selectedRoom.precioPorNoche}</strong>
            </p>
            <p>
              Total estimado:{" "}
              <strong className="text-[#FF9900]">${calcularTotal()}</strong>
            </p>
          </div>
        )}

        {/* Botón Guardar */}
        <button
          onClick={handleSubmit}
          disabled={!disponible}
          className={`w-full mt-6 py-3 rounded font-semibold
            ${
              disponible
                ? "bg-[#FF9900] hover:bg-[#D88000]"
                : "bg-gray-600 cursor-not-allowed"
            }
          `}
        >
          Confirmar Reserva
        </button>
      </div>
    </LayoutDashboard>
  );
}
