import { useNavigate } from "react-router-dom";
import LayoutDashboard from "../../usuarios/components/LayoutDashboard";
import { useEffect, useState } from "react";
import { getRooms } from "../../../api/habitaciones.api";
import { getCategorias } from "../../../api/categorias.api";
import { getReservationById } from "../../../api/reservas.api";
import EditCheckModal from "../components/EditCheckModal";

import {
    getAllCheckInOut,
    updateHistoryRecord,
    deleteHistoryRecord,
} from "../../../api/checkInOut.api";

export default function CheckPage() {
    const navigate = useNavigate();

    const [checks, setChecks] = useState([]);
    const [rooms, setRooms] = useState([]);
    const [categorias, setCategorias] = useState([]);

    const [lastEvent, setLastEvent] = useState(null);
    const [animate, setAnimate] = useState(false);

    // Modal
    const [openModal, setOpenModal] = useState(false);
    const [selected, setSelected] = useState(null);
    const [selectedRoom, setSelectedRoom] = useState(null);
    const [selectedCategoria, setSelectedCategoria] = useState(null);

    // Cargar registros CheckInOut y sus habitaciones
    const loadChecks = async () => {
        const res = await getAllCheckInOut();
        const rawChecks = res.data;

        const checksWithRoom = await Promise.all(
            rawChecks.map(async (c) => {
                const reserva = await getReservationById(c.reservaId);
                return {
                    ...c,
                    habitacionId: reserva.data.habitacionId,
                };
            })
        );

        setChecks(checksWithRoom);
    };

    // Cargar habitaciones y categorías
    useEffect(() => {
        loadChecks();

        getRooms().then((res) => setRooms(res.data.data));
        getCategorias().then((res) => setCategorias(res.data.data));
    }, []);

    // Detectar ultimo evento
    useEffect(() => {
        if (checks.length > 0) {
            const last = [...checks].sort(
                (a, b) => new Date(b.fechaCheckIn) - new Date(a.fechaCheckIn)
            )[0];

            setLastEvent(last);

            setAnimate(true);
            setTimeout(() => setAnimate(false), 500);
        }
    }, [checks]);

    // Abrir modal editar
    const openEdit = (record) => {
        const room = rooms.find((r) => r.id === record.habitacionId);
        const categoria = categorias.find((c) => c.id === room?.categoriaId);

        setSelected(record);
        setSelectedRoom(room);
        setSelectedCategoria(categoria);
        setOpenModal(true);
    };

    const closeModal = () => setOpenModal(false);

    // Guardar cambios
    const handleSave = async (form) => {
        await updateHistoryRecord(selected.id, form);
        closeModal();
        loadChecks();
    };

    // Eliminar registro
    const handleDelete = async (id) => {
        if (!window.confirm("¿Seguro que deseas eliminar este registro?")) return;

        try {
            await deleteHistoryRecord(id);
            loadChecks();
        } catch (error) {
            console.error("Error", error);
            alert("No se pudo eliminar el registro.");
        }
    };

    return (
        <LayoutDashboard>
            <div className="max-w-4xl mx-auto">

                <h1 className="text-3xl font-bold mb-8">Check-In / Check-Out</h1>

                {/* Tarjetas */}
                <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
                    <button
                        onClick={() => navigate("/checkin")}
                        className="bg-[#1A2E44] p-8 rounded-xl shadow-lg hover:bg-[#243b56] text-center"
                    >
                        <h3 className="text-xl font-semibold text-[#FF9900]">Registrar Check-In</h3>
                        <p className="text-gray-300 mt-2">Registrar llegada del huésped</p>
                    </button>

                    <button
                        onClick={() => navigate("/checkout")}
                        className="bg-[#1A2E44] p-8 rounded-xl shadow-lg hover:bg-[#243b56] text-center"
                    >
                        <h3 className="text-xl font-semibold text-[#FF9900]">Registrar Check-Out</h3>
                        <p className="text-gray-300 mt-2">Registrar salida del huésped</p>
                    </button>

                    <button
                        onClick={() => navigate("/historial")}
                        className="bg-[#1A2E44] p-8 rounded-xl shadow-lg hover:bg-[#243b56] text-center"
                    >
                        <h3 className="text-xl font-semibold text-[#FF9900]">Historial Reservas</h3>
                        <p className="text-gray-300 mt-2 whitespace-nowrap">
                            Consulta estancias completadas
                        </p>
                    </button>
                </div>

                {/* Tabla */}
                {lastEvent && (
                    <div
                        className={`mt-16 transition-all duration-500 ${animate ? "opacity-0 translate-y-3" : "opacity-100 translate-y-0"
                            }`}
                    >
                        <h2 className="text-2xl font-bold mb-4">
                            {lastEvent.fechaCheckOut
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
                                    {checks.map((h) => {
                                        const room = rooms.find((r) => r.id === h.habitacionId);
                                        const categoria = categorias.find(
                                            (c) => c.id === room?.categoriaId
                                        );

                                        return (
                                            <tr
                                                key={h.id}
                                                className="border-b border-[#243b56] hover:bg-[#223650] transition"
                                            >
                                                <td className="p-3 font-semibold">
                                                    {room ? `Hab. ${room.numero}` : "—"}
                                                </td>

                                                <td className="p-3 font-semibold text-[#FF9900]">
                                                    {categoria ? `$${categoria.precioPorNoche}` : "—"}
                                                </td>

                                                <td className="p-3">
                                                    {h.fechaCheckIn ? (
                                                        <span className="text-green-400 font-semibold">
                                                            Completado ✓
                                                        </span>
                                                    ) : (
                                                        <span className="text-yellow-400">Pendiente</span>
                                                    )}
                                                </td>

                                                <td className="p-3">
                                                    {h.fechaCheckOut ? (
                                                        <span className="text-green-400 font-semibold">
                                                            Completado ✓
                                                        </span>
                                                    ) : (
                                                        <span className="text-yellow-400">Pendiente</span>
                                                    )}
                                                </td>

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
                    categoria={selectedCategoria}
                    onSave={handleSave}
                />
            </div>
        </LayoutDashboard>
    );
}
