import { useEffect, useState } from "react";
import {
  insertClient,
  getClientById,
  modifyClient,
} from "../../../api/clients.api";
import { useNavigate, useParams } from "react-router-dom";
import LayoutDashboard from "../../usuarios/components/LayoutDashboard";

export default function ClienteForm() {
  const [client, setClient] = useState({
    nombre: "",
    apellido: "",
    correo: "",
    telefono: "",
    documentoIdentidad: "",
  });

  const [errors, setErrors] = useState({});
  const navigate = useNavigate();
  const { id } = useParams();
  const editing = !!id;

  useEffect(() => {
    if (id) {
      getClientById(id).then((res) => {
        setClient(res.data.data);
      });
    } else {
      setClient({
        nombre: "",
        apellido: "",
        correo: "",
        telefono: "",
        documentoIdentidad: "",
      });
    }
  }, [id]);

  const handleChange = (e) => {
    setClient({
      ...client,
      [e.target.name]: e.target.value,
    });
  };

  // VALIDACIONES
  const validate = () => {
    const newErrors = {};

    if (!client.nombre.trim()) newErrors.nombre = "El nombre es obligatorio.";
    if (!client.apellido.trim())
      newErrors.apellido = "El apellido es obligatorio.";

    if (client.correo && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(client.correo)) {
      newErrors.correo = "Formato de correo inválido.";
    }

    if (
      client.telefono &&
      !/^\d{10}$/.test(client.telefono.replace(/-/g, ""))
    ) {
      newErrors.telefono = "El teléfono debe tener 10 dígitos.";
    }

    if (
      client.documentoIdentidad &&
      !/^\d{3}-\d{7}-\d{1}$/.test(client.documentoIdentidad)
    ) {
      newErrors.documentoIdentidad = "Formato debe ser 000-0000000-0";
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0; // true si todo OK
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!validate()) return;

    if (editing) {
      await modifyClient(id, client);
    } else {
      await insertClient(client);
    }

    navigate("/clientes");
  };

  return (
    <LayoutDashboard>
      <div className="max-w-lg mx-auto bg-[#1A2E44] p-8 rounded-xl shadow-lg">
        <h1 className="text-2xl font-bold mb-6">
          {editing ? "Editar Cliente" : "Nuevo Cliente"}
        </h1>

        <form onSubmit={handleSubmit} className="flex flex-col gap-4">
          {/* Nombre */}
          <div>
            <input
              type="text"
              name="nombre"
              placeholder="Nombre"
              value={client.nombre}
              onChange={handleChange}
              className="bg-[#0F1A2B] p-3 rounded text-white w-full"
            />
            {errors.nombre && (
              <p className="text-red-400 text-sm">{errors.nombre}</p>
            )}
          </div>

          {/* Apellido */}
          <div>
            <input
              type="text"
              name="apellido"
              placeholder="Apellido"
              value={client.apellido}
              onChange={handleChange}
              className="bg-[#0F1A2B] p-3 rounded text-white w-full"
            />
            {errors.apellido && (
              <p className="text-red-400 text-sm">{errors.apellido}</p>
            )}
          </div>

          {/* Correo */}
          <div>
            <input
              type="email"
              name="correo"
              placeholder="Correo"
              value={client.correo}
              onChange={handleChange}
              className="bg-[#0F1A2B] p-3 rounded text-white w-full"
            />
            {errors.correo && (
              <p className="text-red-400 text-sm">{errors.correo}</p>
            )}
          </div>

          {/* Teléfono */}
          <div>
            <input
              type="text"
              name="telefono"
              placeholder="Teléfono (10 dígitos)"
              value={client.telefono}
              onChange={handleChange}
              className="bg-[#0F1A2B] p-3 rounded text-white w-full"
            />
            {errors.telefono && (
              <p className="text-red-400 text-sm">{errors.telefono}</p>
            )}
          </div>

          {/* Documento Identidad */}
          <div>
            <input
              type="text"
              name="documentoIdentidad"
              placeholder="000-0000000-0"
              value={client.documentoIdentidad}
              onChange={handleChange}
              className="bg-[#0F1A2B] p-3 rounded text-white w-full"
            />
            {errors.documentoIdentidad && (
              <p className="text-red-400 text-sm">
                {errors.documentoIdentidad}
              </p>
            )}
          </div>

          <button className="bg-[#FF9900] hover:bg-[#D88000] py-3 rounded text-white font-semibold">
            Guardar
          </button>
        </form>
      </div>
    </LayoutDashboard>
  );
}
