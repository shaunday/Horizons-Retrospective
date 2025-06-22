import React from 'react';
import { Text, Center, Paper, Stack, Button } from '@mantine/core';
import { TbChartLine } from "react-icons/tb";

function TradesErrorState() {
  return (
    <Center className="min-h-[60vh] bg-stone-50 w-full pb-50">
      <Paper 
        shadow="md" 
        p="xl" 
        radius="md" 
        className="bg-white border border-red-200 w-full max-w-2xl mx-auto"
      >
        <Stack align="center" gap="md">
          <div className="bg-red-100 p-3 rounded-full">
            <TbChartLine size={32} className="text-red-600" />
          </div>
          <Text size="lg" fw={500} c="red">
            Error loading trades
          </Text>
          <Text size="sm" c="dimmed" ta="center">
            Please try again later or check your connection
          </Text>
          <Button 
            variant="outline" 
            color="red" 
            size="sm"
            onClick={() => window.location.reload()}
          >
            Retry
          </Button>
        </Stack>
      </Paper>
    </Center>
  );
}

export default TradesErrorState; 