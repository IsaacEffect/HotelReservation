import { useState, useEffect } from "react";
import { loginRequest } from "../../api/auth.api";

export const useAuth = () => {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);
  const [isAuthenticated, setIsAuthenticated] = useState(false);

  // Cargar token + usuario al iniciar la app
  useEffect(() => {
    const savedUser = localStorage.getItem("user");
    const token = localStorage.getItem("token");

    if (savedUser && token) {
      setUser(JSON.parse(savedUser));
      setIsAuthenticated(true);
    }

    setLoading(false);
  }, []);

  const login = async (credentials) => {
    const data = await loginRequest(credentials);

    localStorage.setItem("token", data.token);
    localStorage.setItem("user", JSON.stringify(data.user));

    setUser(data.user);
    setIsAuthenticated(true);

    return data;
  };

  const logout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("user");
    setUser(null);
    setIsAuthenticated(false);
  };

  return {
    user,
    login,
    logout,
    isAuthenticated,
    loading
  };
};
