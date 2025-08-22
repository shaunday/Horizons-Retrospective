import { useEffect, useState } from "react";
import { useAuth } from "@hooks/Auth/useAuth";
import { useFetchAndCacheTrades } from "@hooks/Journal/useFetchAndCacheTrades";

function delay(ms) {
  return new Promise((resolve) => setTimeout(resolve, ms));
}

const DELAY_MS = 250;

export function useAppGate() {
  const [authStep, setAuthStep] = useState("pending");
  const [tradeStep, setTradeStep] = useState("pending");
  const [allDone, setAllDone] = useState(false);

  const { user, loginAsDemo } = useAuth();

  const fetchTradesEnabled = authStep === "success";
  const { isLoading: isTradeLoading, isError: isTradeError } = useFetchAndCacheTrades({
    enabled: fetchTradesEnabled,
  });

  // Run login on mount
  useEffect(() => {
    async function run() {
      setAuthStep("pending");
      setTradeStep("pending");
      setAllDone(false);

      try {
        const data = await Promise.all([
          loginAsDemo(),
          delay(DELAY_MS), // enforce minimum delay
        ]).then((results) => results[0]);

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
    if (!fetchTradesEnabled) return;

    if (isTradeError) {
      setTradeStep("error");
      return;
    }

    let successTimeout;
    let doneTimeout;

    if (!isTradeLoading) {
      // Delay before setting tradeStep to success
      successTimeout = setTimeout(() => {
        setTradeStep("success");

        // Delay before setting allDone true
        doneTimeout = setTimeout(() => setAllDone(true), DELAY_MS);
      }, DELAY_MS);
    } else {
      setTradeStep("pending");
      setAllDone(false);
    }

    return () => {
      clearTimeout(successTimeout);
      clearTimeout(doneTimeout);
    };
  }, [fetchTradesEnabled, isTradeLoading, isTradeError]);

  return { authStep, tradeStep, allDone, user };
}
