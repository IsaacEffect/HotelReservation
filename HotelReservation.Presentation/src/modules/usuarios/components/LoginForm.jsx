export default function LoginForm({ onSubmit, loading, error }) {
  const handleSubmit = (e) => {
    e.preventDefault();
    const correo = e.target.correo.value;
    const contrasena = e.target.contrasena.value;

    onSubmit({ correo, contrasena });
  };

  return (
    <form
      onSubmit={handleSubmit}
      className="bg-white p-6 rounded shadow-md w-full max-w-sm"
    >
      <h2 className="text-2xl font-bold mb-4 text-center">Iniciar Sesión</h2>

      {error && (
        <p className="text-red-500 text-sm text-center mb-3">{error}</p>
      )}

      <input
        type="email"
        name="correo"
        placeholder="Correo"
        className="w-full p-2 border rounded mb-3"
        required
      />

      <input
        type="password"
        name="contrasena"
        placeholder="Contraseña"
        className="w-full p-2 border rounded mb-3"
        required
      />

      <button
        type="submit"
        disabled={loading}
        className="w-full bg-blue-600 text-white py-2 rounded hover:bg-blue-700"
      >
        {loading ? "Cargando..." : "Entrar"}
      </button>
    </form>
  );
}
