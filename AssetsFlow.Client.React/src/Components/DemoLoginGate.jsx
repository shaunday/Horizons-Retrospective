import { useEffect, useState } from "react";
import { useAuth } from "@hooks/Auth/useAuth";
import LoadingState from "@components/Common/LoadingStates/LoadingState";  
import ErrorState from "@components/Common/LoadingStates/ErrorState";      

export default function DemoLoginGate({ children }) {
  const { user, loginAsDemo } = useAuth();
  const [loading, setLoading] = useState(!user);
  const [error, setError] = useState(null);

  useEffect(() => {
    let cancelled = false;

    if (user) {
      setLoading(false);
      setError(null);
      return;
    }

    setLoading(true);
    setError(null);

    loginAsDemo()
      .then(() => {
        if (!cancelled) setLoading(false);
      })
      .catch((err) => {
        if (!cancelled) {
          console.error("Demo login failed:", err);
          setError(err);
          setLoading(false);
        }
      });

    return () => {
      cancelled = true;
    };
  }, [user, loginAsDemo]);

  if (loading) 
    return (
      <LoadingState 
        mainText="Logging in as demo user..."
        subText="Please wait while we set things up for you."
      />
    );

  if (error) 
    return (
      <ErrorState
        mainText="Demo login failed"
        subText="Unable to sign in as demo user. Please try again."
        onRetry={() => {
          setLoading(true);
          setError(null);
          loginAsDemo()
            .then(() => setLoading(false))
            .catch((err) => {
              console.error("Demo login failed:", err);
              setError(err);
              setLoading(false);
            });
        }}
      />
    );

  return children;
}