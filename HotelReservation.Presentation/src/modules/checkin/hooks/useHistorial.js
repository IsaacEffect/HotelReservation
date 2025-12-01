import { useState, useCallback } from "react";
import {
    getHistory,
    getHistoryByClient,
    getHistoryByRoom,
} from "../../../api/checkInOut.api";

export function useHistorial() {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const [data, setData] = useState([]);

    const loadAll = useCallback(async () => {
        setLoading(true);
        try {
            const res = await getHistory();
            setData(res.data);
        } catch {
            setError("Error cargando historial");
        } finally {
            setLoading(false);
        }
    }, []);

    const byClient = async (id) => {
        setLoading(true);
        try {
            const res = await getHistoryByClient(id);
            setData(res.data);
        } catch {
            setError("Error al filtrar por cliente");
        } finally {
            setLoading(false);
        }
    };

    const byRoom = async (id) => {
        setLoading(true);
        try {
            const res = await getHistoryByRoom(id);
            setData(res.data);
        } catch {
            setError("Error al filtrar por habitación");
        } finally {
            setLoading(false);
        }
    };

    return { loading, error, data, loadAll, byClient, byRoom };
}
