import React, { useEffect, useState } from "react";
import { Stepper, Center, Paper, Stack, Text } from "@mantine/core";
import { useAuth } from "@hooks/Auth/useAuth";
import { useFetchAndCacheTrades } from "@hooks/Journal/useFetchAndCacheTrades";
import ErrorState from "@components/Common/LoadingStates/ErrorState";
import JournalView from "@views/JournalView";
import Header from "@views/Header";
import Footer from "@views/Footer";

export default function GlobalAppGate() {
  const { loginAsDemo } = useAuth();
  const [authStep, setAuthStep] = useState("pending"); // pending, success, error

  const { isLoading: isTradeLoading, isError: isTradeError } =
    useFetchAndCacheTrades();

  useEffect(() => {
    let cancelled = false;

    async function initialize() {
      setAuthStep("pending");
      try {
        const data = await loginAsDemo();
        if (!data?.user || !data?.token) {
          throw new Error("Invalid login response");
        }
        if (!cancelled) {
          setAuthStep("success");
        }
      } catch (e) {
        console.error("Demo login failed", e);
        if (!cancelled) {
          setAuthStep("error");
        }
      }
    }

    initialize();

    return () => {
      cancelled = true;
    };
  }, []);

  const tradeStep = isTradeError
    ? "error"
    : isTradeLoading
    ? "pending"
    : "success";
  const allDone = authStep === "success" && tradeStep === "success";

  return (
    <div id="vwrapper">
      <Header />
      <main className="bg-stone-50 px-4 py-12">
        {authStep === "error" && <ErrorState mainText="Demo login failed" />}
        {tradeStep === "error" && (
          <ErrorState mainText="Failed to load trades" />
        )}

        {!allDone && authStep !== "error" && tradeStep !== "error" ? (
          <Center className="w-full h-full">
            <Paper
              shadow="md"
              p="xl"
              radius="md"
              className="bg-white border border-slate-200 w-full max-w-4xl"
            >
              <Stack gap="md">
                <Text fw={600} size="lg">
                  Initializing Application
                </Text>
                <Stepper
                  active={
                    authStep !== "success" ? 0 : tradeStep !== "success" ? 1 : 2
                  }
                  orientation="horizontal"
                >
                  <Stepper.Step
                    label="Authentication"
                    description={
                      authStep === "success" ? "Logged in" : "Logging in"
                    }
                    loading={authStep === "pending"}
                    completed={authStep === "success" ? true : undefined}
                  />
                  <Stepper.Step
                    label="Fetching trades"
                    description={
                      tradeStep === "success"
                        ? "Trades loaded"
                        : "Loading trades"
                    }
                    loading={tradeStep === "pending"}
                    completed={tradeStep === "success" ? true : undefined}
                  />
                  <Stepper.Step label="Ready" description="App is ready" />
                </Stepper>
              </Stack>
            </Paper>
          </Center>
        ) : null}

        {allDone && <JournalView />}
      </main>

      <Footer />
    </div>
  );
}
