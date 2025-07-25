import React from 'react';
import { Text, Center, Paper, Stack } from '@mantine/core';
import { TbChartLine } from "react-icons/tb";

function EmptyState({
  mainText = "No items available",
  subText = "Please add some data to get started.",
  icon: Icon = TbChartLine,
}) {
  return (
    <Center className="min-h-[40vh] w-full">
      <Paper 
        shadow="sm" 
        p="xl" 
        radius="md" 
        className="bg-white border border-slate-200"
      >
        <Stack align="center" gap="md">
          <div className="bg-slate-100 p-3 rounded-full">
            <Icon size={32} className="text-slate-600" />
          </div>
          <Text fz="lg" fw={500} c="dimmed">
            {mainText}
          </Text>
          <Text fz="sm" c="dimmed" ta="center">
            {subText}
          </Text>
        </Stack>
      </Paper>
    </Center>
  );
}

export default EmptyState;