import { useState } from "react";
import { loginRequest } from "../../../api/auth.api";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../../app/context/useAuth";

export const useLogin = () => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const navigate = useNavigate();
  const { login: authLogin } = useAuth(); // <-- usamos el contexto real

  const login = async ({ correo, contrasena }) => {
    try {
      setLoading(true);
      setError("");

      const data = await loginRequest({ correo, contrasena });

      // Guardar token usando el CONTEXTO
      authLogin(data.token); // <-- aquí se activa isAuthenticated = true

      navigate("/"); // Redirigir al dashboard inmediatamente

    } catch (err) {
      setError(err.response?.data?.message || "Credenciales inválidas");
    } finally {
      setLoading(false);
    }
  };

  return { login, loading, error };
};
