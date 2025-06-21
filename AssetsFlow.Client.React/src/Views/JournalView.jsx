import React from 'react';
import FilterControl from "@journalComponents/Filtering/FilterControl";
import TradesGallery from "@journalComponents/TradesGallery";
import { useFetchAndCacheTrades } from "@hooks/Journal/useFetchAndCacheTrades";
import { useAddTrade } from "@hooks/Journal/useAddTrade";
import { Button, Stack } from '@mantine/core';
import { TbPlus } from "react-icons/tb";

function JournalView() {
  const { isLoading, isError, trades } = useFetchAndCacheTrades();
  const { addTrade, isAddingTrade } = useAddTrade();

  const onAddTrade = () => addTrade();

  if (isLoading) return <div>Loading trades...</div>;
  if (isError) return <div>Error fetching trades. Please try again later.</div>;

  return (
    <Stack id="journalMainBody" className="bg-stone-50 p-4 border-t border-slate-200">
      {/* <FilterControl /> */}

      {trades?.length ? (
        <TradesGallery />
      ) : (
        <div>No trades available.</div>
      )}

      <div className="flex justify-center">
        <Button
          leftIcon={<TbPlus size={20} />}
          variant="subtle"
          color="blue"
          size="sm"
          onClick={onAddTrade}
          disabled={isAddingTrade}
          className="shadow-md hover:shadow-lg transition-all duration-200 hover:scale-105 min-w-[280px]"
        >
          {isAddingTrade ? "Adding Trade..." : "+ Add a Trade"}
        </Button>
      </div>
    </Stack>
  );
}

export default JournalView;