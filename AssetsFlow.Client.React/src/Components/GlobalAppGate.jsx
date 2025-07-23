import React, { useEffect, useState } from "react";
import { Stepper, Center, Paper, Stack, Text } from "@mantine/core";
import { useAuth } from "@hooks/Auth/useAuth";
import { useFetchAndCacheTrades } from "@hooks/Journal/useFetchAndCacheTrades";
import ErrorState from "@components/Common/LoadingStates/ErrorState";
import JournalView from "@views/JournalView";
import Header from "@views/Header";
import Footer from "@views/Footer";
import { UserName } from "@constants/constants";

const DELAY_MS = 1000;

function delay(ms) {
  return new Promise((resolve) => setTimeout(resolve, ms));
}

// Helper: wait for process and delay concurrently, resolves when both done
async function waitWithMinDelay(promise, delayMs) {
  const delayPromise = delay(delayMs);
  const result = await promise;
  await delayPromise;
  return result;
}

export default function GlobalAppGate() {
  const { user, loginAsDemo } = useAuth();
  const { isLoading: isTradeLoading, isError: isTradeError } =
    useFetchAndCacheTrades();

  const [authStep, setAuthStep] = useState("pending"); // pending, success, error
  const [tradeStep, setTradeStep] = useState("pending"); // pending, success, error
  const [allDone, setAllDone] = useState(false);

  useEffect(() => {
    async function runSteps() {
      setAuthStep("pending");
      setTradeStep("pending");
      setAllDone(false);

      try {
        // Auth step: wait for login + minimum delay
        const data = await waitWithMinDelay(loginAsDemo(), DELAY_MS);
        if (!data?.user || !data?.token) {
          throw new Error("Invalid login response");
        }
        setAuthStep("success");
      } catch (e) {
        setAuthStep("error");
        return;
      }

      if (isTradeError) {
        setTradeStep("error");
        return;
      }

      // Trades step: wait for trades loading to finish, then minimum delay
      if (isTradeLoading) {
        // Poll until loading done
        await new Promise((resolve) => {
          const interval = setInterval(() => {
            if (!isTradeLoading) {
              clearInterval(interval);
              resolve();
            }
          }, 100);
        });
      }

      // Wait at least delay after trades load finished
      await delay(DELAY_MS);

      setTradeStep("success");

      // Final delay before showing main view
      await delay(DELAY_MS);
      setAllDone(true);
    }

    runSteps();
  }, [isTradeLoading, isTradeError]);

  if (authStep === "error") {
    return (
      <div id="vwrapper">
        <Header />
        <main className="bg-stone-50 px-4 py-12">
          <ErrorState mainText="Demo login failed" />
        </main>
        <Footer />
      </div>
    );
  }

  if (tradeStep === "error") {
    return (
      <div id="vwrapper">
        <Header />
        <main className="bg-stone-50 px-4 py-12">
          <ErrorState mainText="Failed to load trades" />
        </main>
        <Footer />
      </div>
    );
  }

  if (!allDone) {
    return (
      <div id="vwrapper">
        <Header />
        <main className="bg-stone-50 px-4 py-12">
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
                      authStep === "success"
                        ? `Logged in as: [${user?.[UserName] ?? "user"}]`
                        : "Logging in..."
                    }
                    loading={authStep === "pending"}
                    completed={authStep === "success" ? true : undefined}
                  />
                  <Stepper.Step
                    label="Fetching trades..."
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
        </main>
        <Footer />
      </div>
    );
  }

  return (
    <div id="vwrapper">
      <Header />
      <main className="bg-stone-50 px-4 py-12">
        <JournalView />
      </main>
      <Footer />
    </div>
  );
}
