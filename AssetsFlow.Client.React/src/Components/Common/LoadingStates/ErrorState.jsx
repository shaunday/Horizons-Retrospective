import React from 'react';
import { Text, Center, Paper, Stack, Button } from '@mantine/core';
import { TbChartLine } from "react-icons/tb";

function ErrorState({
  mainText = "An error occurred",
  subText = "Please try again later or check your connection",
  icon: Icon = TbChartLine,
  retryText = "Retry",
  onRetry = () => window.location.reload(),
  color = "red",
  minHeight = "60vh",
}) {
  return (
    <Center className={`min-h-[${minHeight}] bg-stone-50 w-full pb-50`}>
      <Paper
        shadow="md"
        p="xl"
        radius="md"
        className={`bg-white border border-${color}-200 w-full max-w-2xl mx-auto`}
      >
        <Stack align="center" gap="md">
          <div className={`bg-${color}-100 p-3 rounded-full`}>
            <Icon size={32} className={`text-${color}-600`} />
          </div>
          <Text size="lg" fw={500} c={color}>
            {mainText}
          </Text>
          <Text size="sm" c="dimmed" ta="center">
            {subText}
          </Text>
          <Button variant="outline" color={color} size="sm" onClick={onRetry}>
            {retryText}
          </Button>
        </Stack>
      </Paper>
    </Center>
  );
}

export default ErrorState;
