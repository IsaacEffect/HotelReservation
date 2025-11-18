import React, { useEffect, useState } from "react";

export default function Facturas() {
  const [facturas, setFacturas] = useState([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    cargar();
  }, []);

  async function cargar() {
    setLoading(true);
    try {
      const res = await fetch("https://localhost:7284/api/facturacion/listar");
      const data = await res.json();
      setFacturas(data);
    } catch (err) {
      console.error(err);
      alert("Error cargando facturas. Revisa la API y CORS.");
    }
    setLoading(false);
  }

  async function descargarPdf(id) {
    try {
      const res = await fetch(
        `https://localhost:7284/api/facturacion/pdf/${id}`
      );
      const blob = await res.blob();
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement("a");
      a.href = url;
      a.download = `Factura_${id}.pdf`;
      a.click();
      window.URL.revokeObjectURL(url);
    } catch (err) {
      console.error(err);
      alert("Error al descargar PDF.");
    }
  }

  return (
    <div className="p-6">
      <h1 className="text-2xl mb-4">Facturas</h1>
      {loading ? (
        <p>Cargando...</p>
      ) : (
        <table className="w-full border">
          <thead>
            <tr className="bg-gray-100">
              <th className="p-2">ID</th>
              <th className="p-2">Reserva</th>
              <th className="p-2">Fecha</th>
              <th className="p-2">Total</th>
              <th className="p-2">Acciones</th>
            </tr>
          </thead>
          <tbody>
            {facturas.length === 0 && (
              <tr>
                <td colSpan="5" className="p-2">
                  No hay facturas
                </td>
              </tr>
            )}
            {facturas.map((f) => (
              <tr key={f.id}>
                <td className="p-2">{f.id}</td>
                <td className="p-2">{f.reservaId}</td>
                <td className="p-2">
                  {new Date(f.fechaEmision).toLocaleString()}
                </td>
                <td className="p-2">{f.montoTotal}</td>
                <td className="p-2">
                  <button
                    onClick={() => descargarPdf(f.id)}
                    className="px-2 py-1 bg-blue-500 text-white rounded"
                  >
                    PDF
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}
