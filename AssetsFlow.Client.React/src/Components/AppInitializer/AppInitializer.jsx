import { useEffect, useState } from "react";
import { useAuth } from "@hooks/Auth/useAuth";
import { useFetchAndCacheTrades } from "@hooks/Journal/useFetchAndCacheTrades";

function delay(ms) {
  return new Promise((resolve) => setTimeout(resolve, ms));
}

const DELAY_MS = 1000;

export function useAppGate() {
  const { user, loginAsDemo } = useAuth();
  const { isLoading: isTradeLoading, isError: isTradeError } = useFetchAndCacheTrades();

  const [authStep, setAuthStep] = useState("pending");
  const [tradeStep, setTradeStep] = useState("pending");
  const [allDone, setAllDone] = useState(false);

  useEffect(() => {
    async function run() {
      setAuthStep("pending");
      setTradeStep("pending");
      setAllDone(false);

      try {
        const data = await Promise.all([
          loginAsDemo(),
          delay(DELAY_MS), // enforce minimum delay
        ]).then(results => results[0]);

        if (!data || !data.user || !data.token) throw new Error("Login failed");
        setAuthStep("success");
      } catch {
        setAuthStep("error");
        return;
      }

      if (isTradeError) {
        setTradeStep("error");
        return;
      }

      // Wait for trades loading to finish
      while (isTradeLoading) {
        await delay(100);
      }

      await delay(DELAY_MS);
      setTradeStep("success");

      await delay(DELAY_MS);
      setAllDone(true);
    }

    run();
  }, [isTradeLoading, isTradeError]);

  return { authStep, tradeStep, allDone, user };
}
