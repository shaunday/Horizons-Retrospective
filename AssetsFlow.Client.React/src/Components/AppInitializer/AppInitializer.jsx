import { useEffect, useState } from "react";
import { useAuth } from "@hooks/Auth/useAuth";
import { useFetchAndCacheTrades } from "@hooks/Journal/useFetchAndCacheTrades";
import { useUserData } from "@hooks/UserManagement/useUserData";

function delay(ms) {
  return new Promise((resolve) => setTimeout(resolve, ms));
}

const DELAY_MS = 250;

export function useAppGate() {
  const [authStep, setAuthStep] = useState("pending");
  const [userDataStep, setUserDataStep] = useState("pending");
  const [allDone, setAllDone] = useState(false);

  const { loginAsDemo } = useAuth();

  const isUserAuthenticated = authStep === "success";

  const { isLoading: isFetchTradesLoading, isError: isFetchTradesError } = useFetchAndCacheTrades({
    enabled: isUserAuthenticated,
  });
  const { isLoading: isUserDataLoading, isError: isUserDataError } = useUserData({
    enabled: isUserAuthenticated,
  });

  // Run login on mount
  useEffect(() => {
    async function run() {
      setAuthStep("pending");
      setUserDataStep("pending");
      setAllDone(false);

      try {
        const data = await Promise.all([loginAsDemo(), delay(DELAY_MS)]).then(
          (results) => results[0]
        );

        if (!data || !data.user || !data.token) throw new Error("Login failed");
        setAuthStep("success");
      } catch {
        setAuthStep("error");
      }
    }

    run();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  useEffect(() => {
    if (!isUserAuthenticated) return;

    if (isUserDataError || isFetchTradesError) {
      setUserDataStep("error");
      return;
    }

    let successTimeout;
    let doneTimeout;

    if (!isUserDataLoading && !isFetchTradesLoading) {
      successTimeout = setTimeout(() => {
        setUserDataStep("success");

        doneTimeout = setTimeout(() => setAllDone(true), DELAY_MS);
      }, DELAY_MS);
    } else {
      setUserDataStep("pending");
      setAllDone(false);
    }

    return () => {
      clearTimeout(successTimeout);
      clearTimeout(doneTimeout);
    };
  }, [
    isUserAuthenticated,
    isUserDataLoading,
    isUserDataError,
    isFetchTradesLoading,
    isFetchTradesError,
  ]);

  return { authStep, userDataStep, allDone };
}
