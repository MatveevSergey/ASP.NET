import type { ComponentType } from "react";
import { useSelector } from "react-redux";
import { Navigate } from "react-router-dom";
import type { RootState } from "../store.ts";

export function withAuth(Component: ComponentType) {
  return function AuthedComponent() {
    const currentEmail = useSelector((state: RootState) => state.auth.currentEmail);

    if (!currentEmail) {
      return <Navigate to="/login" replace />;
    }

    return <Component />;
  };
}
