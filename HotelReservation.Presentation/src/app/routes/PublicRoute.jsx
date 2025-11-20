import { Navigate } from "react-router-dom";

export const PublicRoute = ({ isAuth, loading, children }) => {

  // Esperar a que cargue la info de autenticación antes de decidir
  if (loading) {
    return <div style={{ color: "white" }}>Cargando...</div>;
  }

  // Si ya está autenticado, no debe entrar a login, register, etc.
  if (isAuth) {
    return <Navigate to="/" replace />;
  }

  return children;
};
