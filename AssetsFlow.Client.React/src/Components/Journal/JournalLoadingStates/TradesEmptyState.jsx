import React from 'react';
import { Text, Center, Paper, Stack } from '@mantine/core';
import { TbChartLine } from "react-icons/tb";

function TradesEmptyState() {
  return (
    <Center className="min-h-[40vh]">
      <Paper 
        shadow="sm" 
        p="xl" 
        radius="md" 
        className="bg-white border border-slate-200"
      >
        <Stack align="center" gap="md">
          <div className="bg-slate-100 p-3 rounded-full">
            <TbChartLine size={32} className="text-slate-600" />
          </div>
          <Text size="lg" fw={500} c="dimmed">
            No trades available
          </Text>
          <Text size="sm" c="dimmed" ta="center">
            Start by adding your first trade to begin tracking your performance
          </Text>
        </Stack>
      </Paper>
    </Center>
  );
}

export default TradesEmptyState; 