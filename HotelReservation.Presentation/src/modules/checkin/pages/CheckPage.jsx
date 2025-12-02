import { useNavigate } from "react-router-dom";
import LayoutDashboard from "../../usuarios/components/LayoutDashboard";
import { useHistorial } from "../hooks/useHistorial";
import { useEffect, useState } from "react";
import { getRooms } from "../../../api/habitaciones.api";
import EditCheckModal from "../components/EditCheckModal";
import { updateHistoryRecord } from "../../../api/checkInOut.api";

export default function CheckPage() {
    const navigate = useNavigate();

    const { data, loadAll, remove } = useHistorial();
    const [rooms, setRooms] = useState([]);
    const [lastEvent, setLastEvent] = useState(null);
    const [animate, setAnimate] = useState(false);

    // --- Modal states ---
    const [openModal, setOpenModal] = useState(false);
    const [selected, setSelected] = useState(null);
    const [selectedRoom, setSelectedRoom] = useState(null);

    // Cargar historial + habitaciones
    useEffect(() => {
        loadAll();
        getRooms().then((res) => setRooms(res.data.data));
    }, [loadAll]);

    // Detectar ultimo check-in/check-out
    useEffect(() => {
        if (data.length > 0) {
            const last = [...data].sort(
                (a, b) => new Date(b.fechaEntrada) - new Date(a.fechaEntrada)
            )[0];

            setLastEvent(last);

            setAnimate(true);
            const timeout = setTimeout(() => setAnimate(false), 500);
            return () => clearTimeout(timeout);
        }
    }, [data]);

    // --- Abrir modal EDITAR ---
    const openEdit = (record) => {
        const room = rooms.find((r) => r.id === record.habitacionId);
        setSelected(record);
        setSelectedRoom(room);
        setOpenModal(true);
    };

    const closeModal = () => setOpenModal(false);

    // --- Guardar cambios ---
    const handleSave = async (form) => {
        await updateHistoryRecord(selected.id, form);
        closeModal();
        loadAll();
    };

    const handleDelete = async (id) => {
        if (!window.confirm("¿Seguro que deseas eliminar este registro?")) return;
        await remove(id);
        loadAll();
    };

    return (
        <LayoutDashboard>
            <div className="max-w-4xl mx-auto">

                <h1 className="text-3xl font-bold mb-8">Check-In / Check-Out</h1>

                <div className="grid grid-cols-1 md:grid-cols-3 gap-8">

                    {/* CARD CHECK-IN */}
                    <button
                        onClick={() => navigate("/checkin")}
                        className="bg-[#1A2E44] p-8 rounded-xl shadow-lg hover:bg-[#243b56] text-center"
                    >
                        <h3 className="text-xl font-semibold text-[#FF9900]">Registrar Check-In</h3>
                        <p className="text-gray-300 mt-2">Registrar llegada del huésped</p>
                    </button>

                    {/* CARD CHECK-OUT */}
                    <button
                        onClick={() => navigate("/checkout")}
                        className="bg-[#1A2E44] p-8 rounded-xl shadow-lg hover:bg-[#243b56] text-center"
                    >
                        <h3 className="text-xl font-semibold text-[#FF9900]">Registrar Check-Out</h3>
                        <p className="text-gray-300 mt-2">Registrar salida del huésped</p>
                    </button>

                    {/* CARD HISTORIAL */}
                    <button
                        onClick={() => navigate("/historial")}
                        className="bg-[#1A2E44] p-8 rounded-xl shadow-lg hover:bg-[#243b56] text-center"
                    >
                        <h3 className="text-xl font-semibold text-[#FF9900]">Historial Reservas</h3>
                        <p className="text-gray-300 mt-2 whitespace-nowrap">Consulta estancias completadas</p>
                    </button>

                </div>

                {/* TABLA DE ULTIMOS REGISTROS */}
                {lastEvent && (
                    <div
                        className={`mt-16 transition-all duration-500 ${animate ? "opacity-0 translate-y-3" : "opacity-100 translate-y-0"
                            }`}
                    >
                        <h2 className="text-2xl font-bold mb-4">
                            {lastEvent.fechaSalida
                                ? "Se realizó check-out para:"
                                : "Se realizó check-in para:"}
                        </h2>

                        <div className="overflow-x-auto">
                            <table className="min-w-full bg-[#1A2E44] text-white rounded-xl shadow-lg">
                                <thead className="bg-[#0F1A2B]">
                                    <tr>
                                        <th className="p-3 text-left">Habitación</th>
                                        <th className="p-3 text-left">Costo</th>
                                        <th className="p-3 text-left">Check-In</th>
                                        <th className="p-3 text-left">Check-Out</th>
                                        <th className="p-3 text-left">Acciones</th>
                                    </tr>
                                </thead>

                                <tbody>
                                    {data.map((h) => {
                                        const room = rooms.find((r) => r.id === h.habitacionId);

                                        return (
                                            <tr
                                                key={h.id}
                                                className="border-b border-[#243b56] hover:bg-[#223650] transition"
                                            >
                                                {/* Habitacion */}
                                                <td className="p-3 font-semibold">
                                                    {room ? `Hab. ${room.numero}` : "—"}
                                                </td>

                                                {/* Costo */}
                                                <td className="p-3 font-semibold text-[#FF9900]">
                                                    {room ? `$${room.precioPorNoche}` : "—"}
                                                </td>

                                                {/* Check-In */}
                                                <td className="p-3">
                                                    {h.fechaEntrada ? (
                                                        <span className="text-green-400 font-semibold">
                                                            Completado ✓
                                                        </span>
                                                    ) : (
                                                        <span className="text-yellow-400">Pendiente</span>
                                                    )}
                                                </td>

                                                {/* Check-Out */}
                                                <td className="p-3">
                                                    {h.fechaSalida ? (
                                                        <span className="text-green-400 font-semibold">
                                                            Completado ✓
                                                        </span>
                                                    ) : (
                                                        <span className="text-yellow-400">Pendiente</span>
                                                    )}
                                                </td>

                                                {/* Acciones */}
                                                <td className="p-3 flex gap-2">

                                                    <button
                                                        onClick={() => openEdit(h)}
                                                        className="bg-blue-600 hover:bg-blue-700 px-3 py-1 rounded"
                                                    >
                                                        Editar
                                                    </button>

                                                    <button
                                                        onClick={() => handleDelete(h.id)}
                                                        className="bg-red-600 hover:bg-red-700 px-3 py-1 rounded"
                                                    >
                                                        Eliminar
                                                    </button>
                                                </td>
                                            </tr>
                                        );
                                    })}
                                </tbody>
                            </table>
                        </div>

                    </div>
                )}

                <EditCheckModal
                    open={openModal}
                    onClose={closeModal}
                    record={selected}
                    room={selectedRoom}
                    onSave={handleSave}
                />
            </div>
        </LayoutDashboard>
    );
}
