import { useEffect, useState } from "react";
import { loginWithGoogleToken } from "./services/apiClient";
import { useNavigate } from "react-router-dom";


const GOOGLE_CLIENT_ID = "991924951144-qs1d1be85pncp78eh2m29ndrri8qb46h.apps.googleusercontent.com";

export default function Login() {
  const [gsiScriptLoaded, setGsiScriptLoaded] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    if ( gsiScriptLoaded) return

    const initializeGsi = () => {
      // Typescript will complain about window.google
      // Add types to your `react-app-env.d.ts` or //@ts-ignore it.
      if (!window.google || gsiScriptLoaded) return

      setGsiScriptLoaded(true)
      window.google.accounts.id.initialize({
        client_id: GOOGLE_CLIENT_ID,
        callback: handleGoogleSignIn,
      })
    }

    const script = document.createElement("script")
    script.src = "https://accounts.google.com/gsi/client"
    script.onload = initializeGsi
    script.async = true
    script.id = "google-client-script"
    document.querySelector("body")?.appendChild(script)

    return () => {
      // Cleanup function that runs when component unmounts
      window.google?.accounts.id.cancel()
      document.getElementById("google-client-script")?.remove()
    }
  }, [])

  const handleGoogleSignIn = async (res: google.accounts.id.CredentialResponse) => {
    const loginResponse = await loginWithGoogleToken(res.credential);

    localStorage.setItem('token', loginResponse.token);

    navigate('/');
  };

  return <div>
    <h1>Login</h1>
    <div>
      <div className="g_id_signin" />

    </div>
  </div>;
}


interface CredentialResponse extends google.accounts.id.CredentialResponse {
  clientId: string,
  select_by: string
}