import { useEffect, useState } from "react";
import {
    insertCategoria,
    getCategoriaById,
    updateCategoria,
} from "../../../api/categorias.api";
import { useNavigate, useParams } from "react-router-dom";
import LayoutDashboard from "../../usuarios/components/LayoutDashboard";

export default function CategoriaForm() {
    const [categoria, setCategoria] = useState({
        nombreCategoria: "",
        detalle: "",
        precio: "",
    });

    const navigate = useNavigate();
    const { id } = useParams();
    const editing = !!id;

    useEffect(() => {
        if (editing) {
            getCategoriaById(id).then((res) => {
                const c = res.data.data;
                setCategoria({
                    nombreCategoria: c.nombreCategoria,
                    detalle: c.detalle,
                    precio: c.precio,
                });
            });
        }
    }, [id, editing]);

    const handleChange = (e) => {
        setCategoria({
            ...categoria,
            [e.target.name]: e.target.value,
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (editing) {
            await updateCategoria(id, categoria);
        } else {
            await insertCategoria(categoria);
        }

        navigate("/categorias");
    };

    return (
        <LayoutDashboard>
            <div className="max-w-lg mx-auto bg-[#1A2E44] p-8 rounded-xl shadow-lg">
                <h1 className="text-2xl font-bold mb-6">
                    {editing ? "Editar Categoría" : "Nueva Categoría"}
                </h1>

                <form onSubmit={handleSubmit} className="flex flex-col gap-4">
                    <input
                        type="text"
                        name="nombreCategoria"
                        placeholder="Nombre Categoría"
                        value={categoria.nombreCategoria}
                        onChange={handleChange}
                        className="bg-[#0F1A2B] p-3 rounded text-white"
                        required
                    />

                    <input
                        type="text"
                        name="detalle"
                        placeholder="Detalle"
                        value={categoria.detalle}
                        onChange={handleChange}
                        className="bg-[#0F1A2B] p-3 rounded text-white"
                        required
                    />

                    <input
                        type="number"
                        name="precio"
                        placeholder="Precio"
                        value={categoria.precio}
                        onChange={handleChange}
                        className="bg-[#0F1A2B] p-3 rounded text-white"
                        required
                    />

                    <button className="bg-[#FF9900] hover:bg-[#D88000] py-3 rounded text-white font-semibold">
                        Guardar
                    </button>
                </form>
            </div>
        </LayoutDashboard>
    );
}
