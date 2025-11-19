import LoginForm from "../components/LoginForm";
import { useLogin } from "../hooks/useLogin";

export default function LoginPage() {
  const { login, loading, error } = useLogin();

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-100 p-4">
      <LoginForm onSubmit={login} loading={loading} error={error} />
    </div>
  );
}
