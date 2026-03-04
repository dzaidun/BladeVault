import React from "react";
import { LoginForm } from "../components/auth/LoginForm";

export const LoginPage: React.FC = () => {
  return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-slate-900 via-slate-800 to-slate-900 px-4">
      <div className="w-full max-w-md">
        <div className="text-center mb-8">
          <h1 className="text-4xl font-bold text-white mb-2">BladeVault</h1>
          <p className="text-slate-400">Система управління складом та замовленнями</p>
        </div>
        <LoginForm />
      </div>
    </div>
  );
};
