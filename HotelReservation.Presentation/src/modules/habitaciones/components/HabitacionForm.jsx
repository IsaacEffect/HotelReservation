import { useEffect, useState } from "react";
import {
    getHabitacionById,
    insertHabitacion,
    updateHabitacion,
} from "../../../api/habitaciones.api";
import { getCategorias } from "../../../api/categorias.api";
import { useNavigate, useParams } from "react-router-dom";
import LayoutDashboard from "../../usuarios/components/LayoutDashboard";

export default function HabitacionForm() {
    const [categorias, setCategorias] = useState([]);
    const [habitacion, setHabitacion] = useState({
        numero: "",
        detalle: "",
        precio: "",
        estado: "Disponible",
        categoriaId: "",
        piso: "",
    });

    const navigate = useNavigate();
    const { id } = useParams();
    const editing = !!id;

    useEffect(() => {
        getCategorias().then((res) => setCategorias(res.data.data));

        if (editing) {
            getHabitacionById(id).then((res) => {
                const h = res.data.data;
                setHabitacion({
                    numero: h.numero,
                    detalle: h.detalle,
                    precio: h.precio,
                    estado: h.estado,
                    categoriaId: h.categoria.categoriaId,
                    piso: h.piso,
                });
            });
        }
    }, [id, editing]);

    const handleChange = (e) => {
        setHabitacion({
            ...habitacion,
            [e.target.name]: e.target.value,
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (editing) {
            await updateHabitacion(id, habitacion);
        } else {
            await insertHabitacion(habitacion);
        }

        navigate("/habitaciones");
    };

    return (
        <LayoutDashboard>
            <div className="max-w-lg mx-auto bg-[#1A2E44] p-8 rounded-xl shadow-lg">
                <h1 className="text-2xl font-bold mb-6">
                    {editing ? "Editar Habitación" : "Nueva Habitación"}
                </h1>

                <form onSubmit={handleSubmit} className="flex flex-col gap-4">
                    <input
                        type="text"
                        name="numero"
                        placeholder="Número"
                        value={habitacion.numero}
                        onChange={handleChange}
                        className="bg-[#0F1A2B] p-3 rounded text-white"
                        required
                    />

                    <input
                        type="text"
                        name="detalle"
                        placeholder="Detalle"
                        value={habitacion.detalle}
                        onChange={handleChange}
                        className="bg-[#0F1A2B] p-3 rounded text-white"
                        required
                    />

                    <input
                        type="number"
                        name="precio"
                        placeholder="Precio"
                        value={habitacion.precio}
                        onChange={handleChange}
                        className="bg-[#0F1A2B] p-3 rounded text-white"
                        required
                    />

                    <input
                        type="text"
                        name="piso"
                        placeholder="Piso"
                        value={habitacion.piso}
                        onChange={handleChange}
                        className="bg-[#0F1A2B] p-3 rounded text-white"
                        required
                    />

                    <select
                        name="estado"
                        value={habitacion.estado}
                        onChange={handleChange}
                        className="bg-[#0F1A2B] p-3 rounded text-white"
                        required
                    >
                        <option value="Disponible">Disponible</option>
                        <option value="Ocupada">Ocupada</option>
                        <option value="Mantenimiento">Mantenimiento</option>
                        <option value="Limpieza">Limpieza</option>
                    </select>

                    <select
                        name="categoriaId"
                        value={habitacion.categoriaId}
                        onChange={handleChange}
                        className="bg-[#0F1A2B] p-3 rounded text-white"
                        required
                    >
                        <option value="">Seleccione una categoría</option>
                        {categorias.map((c) => (
                            <option key={c.categoriaId} value={c.categoriaId}>
                                {c.nombreCategoria}
                            </option>
                        ))}
                    </select>

                    <button className="bg-[#FF9900] hover:bg-[#D88000] py-3 rounded text-white font-semibold">
                        Guardar
                    </button>
                </form>
            </div>
        </LayoutDashboard>
    );
}
