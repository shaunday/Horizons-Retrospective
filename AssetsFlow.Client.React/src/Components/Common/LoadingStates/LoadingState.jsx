import React from 'react';
import { Loader, Text, Center, Paper, Stack } from '@mantine/core';
import { TbChartLine } from "react-icons/tb";

function LoadingState({ 
  mainText = "Loading...", 
  subText = "Please wait while we process your request.", 
  icon: Icon = TbChartLine 
}) {
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
            <Icon size={32} className="text-slate-600" />
          </div>
          <Loader size="md" color="blue" />
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

export default LoadingState;