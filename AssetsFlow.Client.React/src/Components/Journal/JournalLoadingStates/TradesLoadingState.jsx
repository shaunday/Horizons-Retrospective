import React from 'react';
import { Loader, Text, Center, Paper, Stack } from '@mantine/core';
import { TbChartLine } from "react-icons/tb";

function TradesLoadingState() {
  return (
    <Center className="min-h-[60vh] bg-stone-50 w-full pb-50">
      <Paper 
        shadow="md" 
        p="xl" 
        radius="md" 
        className="bg-white border border-slate-200 w-full max-w-2xl mx-auto"
      >
        <Stack align="center" gap="md">
          <div className="bg-slate-100 p-3 rounded-full">
            <TbChartLine size={32} className="text-slate-600" />
          </div>
          <Loader size="md" color="blue" />
          <Text size="lg" fw={500} c="dimmed">
            Loading your trading journal...
          </Text>
          <Text size="sm" c="dimmed" ta="center">
            Fetching trades and preparing your analysis
          </Text>
        </Stack>
      </Paper>
    </Center>
  );
}

export default TradesLoadingState; 