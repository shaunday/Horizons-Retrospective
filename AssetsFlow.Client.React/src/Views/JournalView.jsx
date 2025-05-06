import React from 'react';
import FilterControl from "@journalComponents/Filtering/FilterControl";
import TradesGallery from "@journalComponents/TradesGallery";
import { useFetchAndCacheTrades } from "@hooks/Journal/useFetchAndCacheTrades";
import { useAddTrade } from "@hooks/Journal/useAddTrade";
import { Button, Stack } from '@mantine/core';

function JournalView() {
  const { isLoading, isError, trades } = useFetchAndCacheTrades();
  const { addTrade, isAddingTrade } = useAddTrade();

  const onAddTrade = () => addTrade();

  if (isLoading) return <div>Loading trades...</div>;
  if (isError) return <div>Error fetching trades. Please try again later.</div>;

  return (
    <Stack id="journalMainBody">
      {/* <FilterControl /> */}

      {trades?.length ? (
        <TradesGallery />
      ) : (
        <div>No trades available.</div>
      )}

      <Button
        className="element-to-be-centered"
        onClick={onAddTrade}
        disabled={isAddingTrade}
      >
        {isAddingTrade ? "Adding Trade..." : "Add a Trade"}
      </Button>
    </Stack>
  );
}

export default JournalView;