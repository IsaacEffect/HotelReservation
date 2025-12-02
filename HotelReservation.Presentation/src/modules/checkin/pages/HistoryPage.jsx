import { useEffect, useState } from "react";
import LayoutDashboard from "../../usuarios/components/LayoutDashboard";
import { useHistorial } from "../hooks/useHistorial";
import { getClients } from "../../../api/clients.api";
import { getRooms } from "../../../api/habitaciones.api";

export default function HistoryPage() {
    const { loading, error, data, loadAll, byClient, byRoom } = useHistorial();

    const [clients, setClients] = useState([]);
    const [rooms, setRooms] = useState([]);

    const [clientId, setClientId] = useState("");
    const [roomId, setRoomId] = useState("");

    useEffect(() => {
        loadAll();
        getClients().then((res) => setClients(res.data.data));
        getRooms().then((res) => setRooms(res.data.data));
    }, [loadAll]);

    const getClientName = (id) => {
        const c = clients.find((x) => x.idCliente === id);
        return c ? `${c.nombre} ${c.apellido}` : "—";
    };

    const getRoomNumber = (id) => {
        const r = rooms.find((x) => x.id === id);
        return r ? r.numero : "—";
    };

    return (
        <LayoutDashboard>
            <div>
                <h1 className="text-2xl font-bold mb-6">Historial de Reservas</h1>

                {/* seleccionar cliente */}
                <div className="flex gap-4 mb-6">
                    <select
                        value={clientId}
                        onChange={(e) => {
                            setClientId(e.target.value);
                            byClient(e.target.value);
                        }}
                        className="bg-[#1A2E44] p-3 rounded text-white w-full border border-[#FF9900]/40"
                    >
                        <option value="">Todos los clientes</option>
                        {clients.map((c) => (
                            <option key={c.idCliente} value={c.idCliente}>
                                {c.nombre} {c.apellido}
                            </option>
                        ))}
                    </select>
                </div>

                {/* seleccionar habitacion */}
                <div className="flex gap-4 mb-6">
                    <select
                        value={roomId}
                        onChange={(e) => {
                            setRoomId(e.target.value);
                            byRoom(e.target.value);
                        }}
                        className="bg-[#1A2E44] p-3 rounded text-white w-full border border-[#FF9900]/40"
                    >
                        <option value="">Todas las habitaciones</option>
                        {rooms.map((h) => (
                            <option key={h.id} value={h.id}>
                                Habitación {h.numero}
                            </option>
                        ))}
                    </select>
                </div>

                {/* tabla */}
                <div className="overflow-x-auto">
                    <table className="min-w-full bg-[#1A2E44] text-white rounded-lg">
                        <thead className="bg-[#0F1A2B]">
                            <tr>
                                <th className="p-3">Cliente</th>
                                <th className="p-3">Habitación</th>
                                <th className="p-3">Entrada</th>
                                <th className="p-3">Salida</th>
                                <th className="p-3">Motivo</th>
                            </tr>
                        </thead>

                        <tbody>
                            {data.map((h) => (
                                <tr key={h.id} className="border-b border-[#243b56]">
                                    <td className="p-3">{getClientName(h.clienteId)}</td>
                                    <td className="p-3">{getRoomNumber(h.habitacionId)}</td>
                                    <td className="p-3">
                                        {new Date(h.fechaEntrada).toLocaleString()}
                                    </td>
                                    <td className="p-3">
                                        {new Date(h.fechaSalida).toLocaleString()}
                                    </td>
                                    <td className="p-3">{h.motivo}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>

                    {loading && (
                        <p className="text-gray-300 mt-4 animate-pulse">
                            Cargando historial...
                        </p>
                    )}

                    {error && <p className="text-red-400 mt-4">{error}</p>}
                </div>
            </div>
        </LayoutDashboard>
    );
}
