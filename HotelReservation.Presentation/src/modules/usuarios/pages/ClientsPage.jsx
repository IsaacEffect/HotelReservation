import { useEffect, useState } from "react";
import { getClients } from "../../../api/clients.api";

export default function ClientsPage() {
  const [clients, setClients] = useState([]);

  useEffect(() => {
    getClients().then((res) => setClients(res.data));
  }, []);

  return (
    <div className="p-4">
      <h1 className="text-xl font-bold mb-4">Clientes</h1>

      <pre>{JSON.stringify(clients, null, 2)}</pre>
    </div>
  );
}
