import React, { useState } from "react";

export default function Reportes() {
    const [desde, setDesde] = useState("");
    const [hasta, setHasta] = useState("");
    const [reporte, setReporte] = useState(null);

    async function obtenerIngresos() {
        if (!desde || !hasta) return alert("Selecciona fechas");
        const res = await fetch(`https://localhost:7284/api/facturacion/report/ingresos?desde=${encodeURIComponent(desde)}&hasta=${encodeURIComponent(hasta)}`);
        const data = await res.json();
        setReporte(data);
    }

    return (
        <div className="p-6">
            <h1 className="text-2xl mb-4">Reportes</h1>
            <div className="mb-4">
                <label>Desde:</label>
                <input type="date" value={desde} onChange={e => setDesde(e.target.value)} />
                <label>Hasta:</label>
                <input type="date" value={hasta} onChange={e => setHasta(e.target.value)} />
                <button onClick={obtenerIngresos} className="ml-2 px-3 py-1 bg-green-600 text-white rounded">Ingresos</button>
            </div>
            <pre>{JSON.stringify(reporte, null, 2)}</pre>
        </div>
    );
}
