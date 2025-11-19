import { Navigate } from "react-router-dom";

export const ProtectedRoute = ({ isAuth, loading, children }) => {
  
  // Mientras esté cargando la autenticación, no redirigir
  if (loading) {
    return <div style={{ color: "white" }}>Cargando...</div>;
  }

  // Ya terminó de cargar: si NO está autenticado, mandar al login
  if (!isAuth) {
    return <Navigate to="/login" replace />;
  }

  // Si está autenticado, renderizar la página protegida
  return children;
};
