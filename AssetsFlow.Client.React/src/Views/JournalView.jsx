import React, { lazy, Suspense, useState } from 'react';
import FilterControl from "@journalComponents/Filtering/FilterControl";
import TradesGallery from "@journalComponents/TradesGallery";
import { useFetchAndCacheTrades } from "@hooks/Journal/useFetchAndCacheTrades";
import { useAddTrade } from "@hooks/Journal/useAddTrade";
import { Button, Stack, Drawer, Group } from '@mantine/core';

const LazyPnLLineChart = lazy(() => import('@components/PnLLineChart'));

function JournalView() {
  const { isLoading, isError, trades } = useFetchAndCacheTrades();
  const { addTrade, isAddingTrade } = useAddTrade();
  
  const [drawerOpen, setDrawerOpen] = useState(false);

  const onAddTrade = () => addTrade();

  if (isLoading) return <div>Loading trades...</div>;
  if (isError) return <div>Error fetching trades. Please try again later.</div>;
  if (!trades?.length) return <div>No trades available.</div>;

  return (
    <Stack id="journalMainBody" >
      {/* <FilterControl /> */}
      
      <TradesGallery />
      
      <Group spacing="sm" position="center" className="element-to-be-centered">
        <Button
          onClick={onAddTrade}
          disabled={isAddingTrade}
        >
          {isAddingTrade ? "Adding Trade..." : "Add a Trade"}
        </Button>

        <Button
          onClick={() => setDrawerOpen(true)} 
        >
          View PnL
        </Button>
      </Group>

      <Drawer
        opened={drawerOpen}
        onClose={() => setDrawerOpen(false)} // Close the drawer when clicking on the backdrop
        title="PnL Line Chart"
        size="lg"
      >
        <Suspense fallback={<div>Loading chart...</div>}>
          <LazyPnLLineChart />
        </Suspense>
      </Drawer>
    </Stack>
  );
}

export default JournalView;