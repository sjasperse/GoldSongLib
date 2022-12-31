import { Navigate } from "react-router";
import { User } from "./types";

type LogoutParams = {
  setUser: (user: User | null) => void
};

export default function Logout({ setUser }: LogoutParams) {
  localStorage.removeItem('token');
  setUser(null);

  return <Navigate to="/" />
}
