import { Navigate } from "react-router-dom";

export const PublicRoute = ({ isAuth, loading, children }) => {
  if (loading) return <div>Cargando...</div>;

  if (isAuth) return <Navigate to="/" replace />;

  return children;
};
