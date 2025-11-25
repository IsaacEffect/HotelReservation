import { BrowserRouter, Routes, Route } from "react-router-dom";
import { ProtectedRoute } from "./ProtectedRoute";
import { PublicRoute } from "./PublicRoute";

import LoginPage from "../../modules/usuarios/pages/LoginPage";
import DashboardPage from "../../modules/usuarios/pages/DashboardPage";
import ClientesPage from "../../modules/usuarios/pages/ClientsPage";
import ClienteForm from "../../modules/usuarios/components/ClientForm";
import ReservasPage from "../../modules/reservas/pages/ReservasPage";
import CheckPage from "../../modules/checkin/pages/CheckPage";
import HabitacionesPage from "../../modules/habitaciones/pages/HabitacionesPage";
import ReportesPage from "../../modules/facturacion/pages/ReportesPage";

import { useAuth } from "../context/useAuth";

export const AppRouter = () => {
  const { isAuthenticated, loading } = useAuth();

  return (
    <BrowserRouter>
      <Routes>
        <Route
          path="/login"
          element={
            <PublicRoute isAuth={isAuthenticated} loading={loading}>
              <LoginPage />
            </PublicRoute>
          }
        />

        <Route
          path="/"
          element={
            <ProtectedRoute isAuth={isAuthenticated} loading={loading}>
              <DashboardPage />
            </ProtectedRoute>
          }
        />

        <Route
          path="/reservas"
          element={
            <ProtectedRoute isAuth={isAuthenticated} loading={loading}>
              <ReservasPage />
            </ProtectedRoute>
          }
        />

        <Route
          path="/check"
          element={
            <ProtectedRoute isAuth={isAuthenticated} loading={loading}>
              <CheckPage />
            </ProtectedRoute>
          }
        />

        <Route
          path="/habitaciones"
          element={
            <ProtectedRoute isAuth={isAuthenticated} loading={loading}>
              <HabitacionesPage />
            </ProtectedRoute>
          }
        />

        <Route
          path="/reportes"
          element={
            <ProtectedRoute isAuth={isAuthenticated} loading={loading}>
              <ReportesPage />
            </ProtectedRoute>
          }
        />
        <Route
          path="/clientes"
          element={
            <ProtectedRoute isAuth={isAuthenticated} loading={loading}>
              <ClientesPage />
            </ProtectedRoute>
          }
        />

        <Route
          path="/clientes/nuevo"
          element={
            <ProtectedRoute isAuth={isAuthenticated} loading={loading}>
              <ClienteForm />
            </ProtectedRoute>
          }
        />

        <Route
          path="/clientes/editar/:id"
          element={
            <ProtectedRoute isAuth={isAuthenticated} loading={loading}>
              <ClienteForm />
            </ProtectedRoute>
          }
        />
      </Routes>
    </BrowserRouter>
  );
};
