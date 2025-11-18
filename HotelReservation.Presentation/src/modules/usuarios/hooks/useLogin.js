import { useState } from "react";
import { loginRequest } from "../../../api/auth.api";
import { useNavigate } from "react-router-dom";

export const useLogin = () => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const navigate = useNavigate();

  const login = async ({ correo, contrasena }) => {
    try {
      setLoading(true);
      setError("");

      const data = await loginRequest({ correo, contrasena });

      // Guardar token + user
      localStorage.setItem("token", data.token);
      localStorage.setItem("user", JSON.stringify(data.user));

      navigate("/"); // Redirigir al dashboard

    } catch (err) {
      setError(err.response?.data?.message || "Credenciales inválidas");
    } finally {
      setLoading(false);
    }
  };

  return { login, loading, error };
};
