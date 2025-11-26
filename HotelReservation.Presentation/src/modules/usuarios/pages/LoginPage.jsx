import LoginForm from "../components/LoginForm";
import { useLogin } from "../hooks/useLogin";

export default function LoginPage() {
  const { login, loading, error } = useLogin();

  return (
    <div className="min-h-screen flex items-center justify-center bg-[#0F1A2B] px-4">
      <div className="max-w-md w-full bg-[#1A2E44] p-10 rounded-2xl shadow-2xl">
        <h1 className="text-3xl font-bold text-center text-white mb-8">
          Bienvenido a <span className="text-[#FF9900]">RoyalKey</span>
        </h1>

        <LoginForm onSubmit={login} loading={loading} error={error} />
      </div>
    </div>
  );
}
