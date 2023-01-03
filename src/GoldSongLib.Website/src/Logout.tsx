import { Navigate } from "react-router";
import { User } from "./types";

type LogoutParams = {
  onLoggedOut: () => void
};

export default function Logout({ onLoggedOut }: LogoutParams) {
  localStorage.removeItem('token');
  onLoggedOut();

  return <Navigate to="/" />
}
