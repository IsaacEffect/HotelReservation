import { useState, useEffect } from "react";

export default function EditCheckModal({ open, onClose, record, room, onSave }) {
    const [form, setForm] = useState({
        fechaEntrada: "",
        fechaSalida: "",
        observaciones: "",
    });

    useEffect(() => {
        if (record) {
            setForm({
                fechaEntrada: record.fechaEntrada?.substring(0, 16) || "",
                fechaSalida: record.fechaSalida?.substring(0, 16) || "",
                observaciones: record.observaciones || "",
            });
        }
    }, [record]);

    if (!open) return null;

    const handleChange = (e) =>
        setForm({ ...form, [e.target.name]: e.target.value });

    const handleSubmit = () => {
        if (form.fechaSalida && form.fechaSalida < form.fechaEntrada) {
            alert("La fecha de salida no puede ser menor que la de entrada.");
            return;
        }
        onSave(form);
    };

    return (
        <div className="fixed inset-0 bg-black/50 backdrop-blur-sm flex items-center justify-center z-50">
            <div className="bg-[#1A2E44] p-8 rounded-xl shadow-xl w-full max-w-lg animate-fadeIn">

                <h2 className="text-2xl font-bold mb-4 text-[#FF9900]">
                    Editar Registro
                </h2>

                <p className="text-gray-300 mb-4">
                    Habitación <strong>{room?.numero}</strong> · Precio:{" "}
                    <strong className="text-[#FF9900]">
                        ${room?.precioPorNoche}
                    </strong>
                </p>

                {/* Formulario */}
                <div className="space-y-4">
                    <div>
                        <label className="text-gray-300 text-sm">Fecha Check-In</label>
                        <input
                            type="datetime-local"
                            name="fechaEntrada"
                            value={form.fechaEntrada}
                            onChange={handleChange}
                            className="bg-[#0F1A2B] p-3 w-full rounded text-white"
                        />
                    </div>

                    <div>
                        <label className="text-gray-300 text-sm">Fecha Check-Out</label>
                        <input
                            type="datetime-local"
                            name="fechaSalida"
                            value={form.fechaSalida}
                            onChange={handleChange}
                            className="bg-[#0F1A2B] p-3 w-full rounded text-white"
                        />
                    </div>

                    <div>
                        <label className="text-gray-300 text-sm">Observaciones</label>
                        <textarea
                            name="observaciones"
                            value={form.observaciones}
                            onChange={handleChange}
                            className="bg-[#0F1A2B] p-3 w-full rounded text-white"
                        ></textarea>
                    </div>
                </div>

                <div className="flex justify-end gap-3 mt-6">
                    <button
                        onClick={onClose}
                        className="px-4 py-2 bg-gray-600 hover:bg-gray-700 rounded"
                    >
                        Cancelar
                    </button>

                    <button
                        onClick={handleSubmit}
                        className="px-4 py-2 bg-[#FF9900] hover:bg-[#D88000] rounded text-white font-bold"
                    >
                        Guardar Cambios
                    </button>
                </div>
            </div>
        </div>
    );
}
