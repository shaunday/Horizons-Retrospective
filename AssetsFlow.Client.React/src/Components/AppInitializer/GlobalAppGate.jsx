import React from "react";
import { Stepper, Center, Paper, Stack, Text } from "@mantine/core";
import { useAppGate } from "./AppInitializer";
import ErrorState from "@components/Common/LoadingStates/ErrorState";
import JournalView from "@views/JournalView";
import Layout from "./Layout";
import { UserName } from "@constants/constants";

export default function GlobalAppGate() {
  const { authStep, tradeStep, allDone, user } = useAppGate();

  if (authStep === "error") {
    return (
      <Layout>
        <ErrorState mainText="Demo login failed" />
      </Layout>
    );
  }

  if (tradeStep === "error") {
    return (
      <Layout>
        <ErrorState mainText="Failed to load trades" />
      </Layout>
    );
  }

  if (!allDone) {
    return (
      <Layout showUserActions={false}>
        <Center className="w-full h-full">
          <Paper
            shadow="md"
            p="xl"
            radius="md"
            className="bg-white border border-slate-200 w-full max-w-4xl"
          >
            <Stack gap="md">
              <Text fw={600} fz="lg">
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
                  {...(authStep === "success" ? { completed: true } : {})}
                />
                <Stepper.Step
                  label="Fetching trades..."
                  description={
                    tradeStep === "success" ? "Trades loaded" : "Loading trades"
                  }
                  loading={tradeStep === "pending"}
                  {...(tradeStep === "success" ? { completed: true } : {})}
                />
                <Stepper.Step label="Ready" description="App is ready" />
              </Stepper>
            </Stack>
          </Paper>
        </Center>
      </Layout>
    );
  }

  return (
    <Layout>
      <JournalView />
    </Layout>
  );
}
