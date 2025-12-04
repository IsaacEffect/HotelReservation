import { useEffect, useState } from "react";
import { AuthContext } from "./AuthContextInstance";
import { getUserById } from "../../api/users.api";
import { jwtDecode } from "jwt-decode";

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [loading, setLoading] = useState(true);

  const fetchUser = async (token) => {
    try {
      const decoded = jwtDecode(token);

      const idUsuario =
        decoded[
          "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
        ];

      if (!idUsuario) return;

      const res = await getUserById(idUsuario);
      const usuarioBD = res.data.data;

      setUser(usuarioBD);
    } catch (err) {
      console.error("Error cargando usuario:", err);
    }
  };

  useEffect(() => {
    const token = localStorage.getItem("token");

    if (token) {
      setIsAuthenticated(true);
      fetchUser(token);
    } else {
      setIsAuthenticated(false);
      setUser(null);
    }

    setLoading(false);
  }, []);

  const login = async (token) => {
    localStorage.setItem("token", token);
    setIsAuthenticated(true);
    await fetchUser(token);
  };

  const logout = () => {
    localStorage.removeItem("token");
    setUser(null);
    setIsAuthenticated(false);
  };

  return (
    <AuthContext.Provider
      value={{ user, isAuthenticated, loading, login, logout }}
    >
      {children}
    </AuthContext.Provider>
  );
};
