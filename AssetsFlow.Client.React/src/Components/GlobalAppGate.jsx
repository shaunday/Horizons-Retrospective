// GlobalAppGate.jsx
import React, { useEffect, useState } from "react";
import { Stepper, Center, Paper, Stack, Text } from "@mantine/core";
import { useAuth } from "@hooks/Auth/useAuth";
import { useFetchAndCacheTrades } from "@hooks/Journal/useFetchAndCacheTrades";
import ErrorState from "@components/Common/LoadingStates/ErrorState";
import JournalView from "@views/JournalView";
import Header from "@views/Header";
import Footer from "@views/Footer";

export default function GlobalAppGate() {
  const { user, loginAsDemo } = useAuth();
  const [authStep, setAuthStep] = useState("pending"); // pending, success, error

  const {
    isLoading: isTradeLoading,
    isError: isTradeError,
    trades,
  } = useFetchAndCacheTrades();

  useEffect(() => {
    async function initialize() {
      if (!user) {
        try {
          setAuthStep("pending");
          await loginAsDemo();
          setAuthStep("success");
        } catch (e) {
          console.error("Demo login failed", e);
          setAuthStep("error");
        }
      } else {
        setAuthStep("success");
      }
    }
    initialize();
  }, [user, loginAsDemo]);

  const tradeStep = isTradeError
    ? "error"
    : isTradeLoading
    ? "pending"
    : "success";
  const allDone = authStep === "success" && tradeStep === "success";

  return (
    <div id="vwrapper">
      <Header />
      <main className="min-h-screen bg-stone-50 px-4 py-12">
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
              className="bg-white border border-slate-200 max-w-md w-full"
            >
              <Stack gap="md">
                <Text fw={600} size="lg">
                  Initializing Application
                </Text>
                <Stepper
                  active={
                    authStep !== "success" ? 0 : tradeStep !== "success" ? 1 : 2
                  }
                  orientation="vertical"
                >
                  <Stepper.Step
                    label="Authentication"
                    description={
                      authStep === "success" ? "Logged in" : "Logging in"
                    }
                    loading={authStep === "pending"}
                    completed={authStep === "success"}
                  />
                  <Stepper.Step
                    label="Fetching trades"
                    description={
                      tradeStep === "success"
                        ? "Trades loaded"
                        : "Loading trades"
                    }
                    loading={tradeStep === "pending"}
                    completed={tradeStep === "success"}
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
