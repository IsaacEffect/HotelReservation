import { AppRouter } from "./app/routes/AppRouter";
import { useAuth } from "./app/hooks/useAuth";

function App() {
  const { isAuthenticated, loading } = useAuth();

  return <AppRouter isAuth={isAuthenticated} loading={loading} />;
}

export default App;
