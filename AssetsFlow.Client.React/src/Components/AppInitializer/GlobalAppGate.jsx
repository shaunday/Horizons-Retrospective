import React from "react";
import { Stepper, Center, Paper, Stack, Text, Progress } from "@mantine/core";
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
    const activeStep =
      authStep !== "success" ? 0 : tradeStep !== "success" ? 1 : 2;

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
                active={activeStep}
                orientation="horizontal"
                breakpoint="sm"
              >
                <Stepper.Step
                  label="Authentication"
                  description={
                    authStep === "success"
                      ? `Logged in as: [${user?.[UserName] ?? "user"}]`
                      : "Logging in..."
                  }
                  loading={authStep === "pending"}
                  completed={authStep === "success" ? "true" : "false"}
                />
                <Stepper.Step
                  label="Fetching trades..."
                  description={
                    tradeStep === "success" ? "Trades loaded" : "Loading trades"
                  }
                  loading={tradeStep === "pending"}
                  completed={tradeStep === "success"? "true" : "false"}
                />
                <Stepper.Step
                  label="Ready"
                  description="App is ready"
                  completed="false"
                />
              </Stepper>

              <Progress
                value={activeStep === 0 ? 0 : activeStep === 1 ? 50 : 100}
                size="md"
                radius="xl"
                mt="md"
                animate="true"
                color="blue"
              />
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
