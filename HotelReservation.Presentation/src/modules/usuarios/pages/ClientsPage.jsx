import { useEffect, useState } from "react";
import { getClients, deleteClient } from "../../../api/clients.api";
import { useNavigate } from "react-router-dom";
import LayoutDashboard from "../../usuarios/components/LayoutDashboard";

export default function ClientsPage() {
  const [clients, setClients] = useState([]);
  const navigate = useNavigate();

  const loadClients = () => {
    getClients().then((res) => {
      setClients(res.data.data); // ← AQUI ESTA LA CORRECCIÓN
    });
  };

  useEffect(() => {
    loadClients();
  }, []);

  const handleDelete = async (id) => {
    const confirmDelete = window.confirm(
      "¿Seguro que deseas eliminar este cliente?"
    );
    if (!confirmDelete) return;

    await deleteClient(id);
    loadClients();
  };

  return (
    <LayoutDashboard>
      <div>
        <div className="flex justify-between items-center mb-6">
          <h1 className="text-2xl font-bold">Clientes</h1>

          <button
            onClick={() => navigate("/clientes/nuevo")}
            className="bg-[#FF9900] hover:bg-[#D88000] px-4 py-2 rounded text-white font-semibold"
          >
            Nuevo Cliente
          </button>
        </div>

        <div className="overflow-x-auto">
          <table className="min-w-full bg-[#1A2E44] text-white rounded-lg overflow-hidden">
            <thead className="bg-[#0F1A2B]">
              <tr>
                <th className="p-3 text-left">Nombre</th>
                <th className="p-3 text-left">Correo</th>
                <th className="p-3 text-left">Teléfono</th>
                <th className="p-3 text-left">Documento</th>
                <th className="p-3 text-left">Acciones</th>
              </tr>
            </thead>

            <tbody>
              {clients.map((c) => (
                <tr key={c.idCliente} className="border-b border-[#243b56]">
                  <td className="p-3">
                    {c.nombre} {c.apellido}
                  </td>
                  <td className="p-3">{c.correo}</td>
                  <td className="p-3">{c.telefono}</td>
                  <td className="p-3">{c.documentoIdentidad}</td>

                  <td className="p-3 flex gap-2">
                    <button
                      className="bg-blue-600 hover:bg-blue-700 px-3 py-1 rounded text-white"
                      onClick={() =>
                        navigate(`/clientes/editar/${c.idCliente}`)
                      }
                    >
                      Editar
                    </button>

                    <button
                      className="bg-red-600 hover:bg-red-700 px-3 py-1 rounded text-white"
                      onClick={() => handleDelete(c.idCliente)}
                    >
                      Eliminar
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>

          {clients.length === 0 && (
            <p className="text-center text-gray-300 mt-6">
              No hay clientes registrados.
            </p>
          )}
        </div>
      </div>
    </LayoutDashboard>
  );
}
