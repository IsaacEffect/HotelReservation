import { useState } from "react";
import { registerCheckIn } from "../../../api/checkInOut.api";

export function useCheckIn() {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const [result, setResult] = useState(null);

    const submit = async (payload) => {
        setLoading(true);
        setError(null);

        try {
            const res = await registerCheckIn(payload);
            setResult(res.data);
        } catch (err) {
            setError(err?.message || "Error en Check-In");
        } finally {
            setLoading(false);
        }
    };

    return { loading, error, result, submit };
}
