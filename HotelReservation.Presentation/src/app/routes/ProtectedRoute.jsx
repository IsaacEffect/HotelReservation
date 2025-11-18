import { Navigate } from "react-router-dom";

export const ProtectedRoute = ({ isAuth, loading, children }) => {
  if (loading) return <div>Cargando...</div>;

  if (!isAuth) return <Navigate to="/login" replace />;

  return children;
};
