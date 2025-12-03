import { useState } from "react";
import { registerCheckOut } from "../../../api/checkInOut.api";

export function useCheckOut() {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);
    const [result, setResult] = useState(null);

    const submit = async (payload) => {
        setLoading(true);
        setError(null);

        try {
            const res = await registerCheckOut(payload);
            setResult(res.data);
        } catch (err) {
            setError(err?.message || "Error en Check-Out");
        } finally {
            setLoading(false);
        }
    };

    return { loading, error, result, submit };
}
