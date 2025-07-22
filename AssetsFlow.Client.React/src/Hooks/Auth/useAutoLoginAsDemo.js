import { useEffect } from "react";
import { useAuth } from "./useAuth";

export function useAutoLoginAsDemo() {
  const { user, loginAsDemo } = useAuth();

  useEffect(() => {
    if (!user) {
      loginAsDemo()
        .then(() => console.log("Logged in as demo"))
        .catch((err) => console.error("Demo login failed", err));
    }
  }, [user, loginAsDemo]);
}
