export default function LoginForm({ onSubmit, loading, error }) {
  const handleSubmit = (e) => {
    e.preventDefault();
    const correo = e.target.correo.value;
    const contrasena = e.target.contrasena.value;

    onSubmit({ correo, contrasena });
  };

  return (
    <form onSubmit={handleSubmit} className="flex flex-col gap-5">
      {error && (
        <p className="text-red-400 text-center bg-red-900/30 p-2 rounded">
          {error}
        </p>
      )}

      {/* Campo Correo */}
      <div className="flex flex-col gap-1">
        <label className="text-white font-medium">Correo</label>
        <input
          type="email"
          name="correo"
          placeholder="ejemplo@correo.com"
          className="w-full p-3 rounded-lg bg-[#1A2E44] border border-transparent 
                     text-white placeholder-gray-400 focus:border-[#FF9900] focus:outline-none"
          required
        />
      </div>

      {/* Campo Contraseña */}
      <div className="flex flex-col gap-1">
        <label className="text-white font-medium">Contraseña</label>
        <input
          type="password"
          name="contrasena"
          placeholder="••••••••"
          className="w-full p-3 rounded-lg bg-[#1A2E44] border border-transparent 
                     text-white placeholder-gray-400 focus:border-[#FF9900] focus:outline-none"
          required
        />
      </div>

      {/* Botón */}
      <button
        type="submit"
        disabled={loading}
        className="w-full py-3 rounded-lg font-semibold text-white 
                   bg-[#FF9900] hover:bg-[#D88000] transition-all shadow-lg"
      >
        {loading ? "Cargando..." : "Ingresar"}
      </button>

      {/* Enlace olvidé mi contraseña */}
      <p className="text-center text-sm text-[#FF9900] cursor-pointer hover:underline">
        ¿Olvidaste tu contraseña?
      </p>
    </form>
  );
}
