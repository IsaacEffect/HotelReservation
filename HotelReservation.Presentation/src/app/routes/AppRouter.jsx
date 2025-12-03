import { BrowserRouter, Routes, Route } from "react-router-dom";
import { ProtectedRoute } from "./ProtectedRoute";
import { PublicRoute } from "./PublicRoute";

import LoginPage from "../../modules/usuarios/pages/LoginPage";
import DashboardPage from "../../modules/usuarios/pages/DashboardPage";
import UsersPage from "../../modules/usuarios/pages/UsersPage";
import UserForm from "../../modules/usuarios/components/UserForm";
import ChangePassword from "../../modules/usuarios/pages/ChangePassword";
import ClientesPage from "../../modules/usuarios/pages/ClientsPage";
import ClienteForm from "../../modules/usuarios/components/ClientForm";
import RolesPage from "../../modules/usuarios/pages/RolesPage";
import RoleForm from "../../modules/usuarios/components/RoleForm";
import ReservasPage from "../../modules/reservas/pages/ReservasPage";
import ReservaForm from "../../modules/reservas/components/ReservaForm";
import ReservaDetallePage from "../../modules/reservas/pages/ReservaDetallePage";
import CheckPage from "../../modules/checkin/pages/CheckPage";
import CheckInPage from "../../modules/checkin/pages/CheckInPage";
import CheckOutPage from "../../modules/checkin/pages/CheckOutPage";
import HistoryPage from "../../modules/checkin/pages/HistoryPage";
import HabitacionesPage from "../../modules/habitaciones/pages/HabitacionesPage";
import HabitacionForm from "../../modules/habitaciones/components/HabitacionForm";
import CategoriasPage from "../../modules/categorias/pages/CategoriasPage";
import CategoriaForm from "../../modules/categorias/components/CategoriaForm";
import FacturasListado from "../../modules/facturacion/pages/FacturasListado";
import FacturaCrear from "../../modules/facturacion/pages/FacturaCrear";
import FacturaDetalle from "../../modules/facturacion/pages/FacturaDetalle";
import FacturasReportes from "../../modules/facturacion/pages/FacturasReportes";

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
          path="/reservas/nueva"
          element={
            <ProtectedRoute isAuth={isAuthenticated} loading={loading}>
              <ReservaForm />
            </ProtectedRoute>
          }
        />

        <Route
          path="/reservas/detalle/:id"
          element={
            <ProtectedRoute isAuth={isAuthenticated} loading={loading}>
              <ReservaDetallePage />
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
          path="/checkin"
          element={
            <ProtectedRoute isAuth={isAuthenticated} loading={loading}>
              <CheckInPage />
            </ProtectedRoute>
          }
        />

        <Route
          path="/checkout"
          element={
            <ProtectedRoute isAuth={isAuthenticated} loading={loading}>
              <CheckOutPage />
            </ProtectedRoute>
          }
        />

        <Route
          path="/historial"
          element={
            <ProtectedRoute isAuth={isAuthenticated} loading={loading}>
              <HistoryPage />
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
          path="/habitaciones/nueva"
          element={
            <ProtectedRoute isAuth={isAuthenticated} loading={loading}>
              <HabitacionForm />
            </ProtectedRoute>
          }
        />

        <Route
          path="/habitaciones/editar/:id"
          element={
            <ProtectedRoute isAuth={isAuthenticated} loading={loading}>
              <HabitacionForm />
            </ProtectedRoute>
          }
        />

        <Route
          path="/categorias"
          element={
            <ProtectedRoute isAuth={isAuthenticated} loading={loading}>
              <CategoriasPage />
            </ProtectedRoute>
          }
        />

        <Route
          path="/categorias/nueva"
          element={
            <ProtectedRoute isAuth={isAuthenticated} loading={loading}>
              <CategoriaForm />
            </ProtectedRoute>
          }
        />

        <Route
          path="/categorias/editar/:id"
          element={
            <ProtectedRoute isAuth={isAuthenticated} loading={loading}>
              <CategoriaForm />
            </ProtectedRoute>
          }
        />

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

        <Route
          path="/usuarios"
          element={
            <ProtectedRoute isAuth={isAuthenticated} loading={loading}>
              <UsersPage />
            </ProtectedRoute>
          }
        />

        <Route
          path="/usuarios/nuevo"
          element={
            <ProtectedRoute isAuth={isAuthenticated} loading={loading}>
              <UserForm />
            </ProtectedRoute>
          }
        />

        <Route
          path="/usuarios/editar/:id"
          element={
            <ProtectedRoute isAuth={isAuthenticated} loading={loading}>
              <UserForm />
            </ProtectedRoute>
          }
        />

        <Route
          path="/usuarios/cambiar-pass/:id"
          element={
            <ProtectedRoute isAuth={isAuthenticated} loading={loading}>
              <ChangePassword />
            </ProtectedRoute>
          }
        />

        <Route
          path="/roles"
          element={
            <ProtectedRoute isAuth={isAuthenticated} loading={loading}>
              <RolesPage />
            </ProtectedRoute>
          }
        />

        <Route
          path="/roles/nuevo"
          element={
            <ProtectedRoute isAuth={isAuthenticated} loading={loading}>
              <RoleForm />
            </ProtectedRoute>
          }
        />

 {/** ======================= FACTURACIÓN ======================= */}

                <Route
                    path="/facturacion"
                    element={
                        <ProtectedRoute isAuth={isAuthenticated} loading={loading}>
                            <FacturasListado />
                        </ProtectedRoute>
                    }
                />

                <Route
                    path="/facturacion/nueva"
                    element={
                        <ProtectedRoute isAuth={isAuthenticated} loading={loading}>
                            <FacturaCrear />
                        </ProtectedRoute>
                    }
                />

                <Route
                    path="/facturacion/detalle/:id"
                    element={
                        <ProtectedRoute isAuth={isAuthenticated} loading={loading}>
                            <FacturaDetalle />
                        </ProtectedRoute>
                    }
                />

                <Route
                    path="/reportes"
                    element={
                        <ProtectedRoute isAuth={isAuthenticated} loading={loading}>
                            <FacturasReportes />
                        </ProtectedRoute>
                    }
                />
      </Routes>
    </BrowserRouter>
  );
};
