import React from 'react';
import FilterControl from "@journalComponents/Filtering/FilterControl";
import TradesGallery from "@journalComponents/TradesGallery";
import { useFetchAndCacheTrades } from "@hooks/Journal/useFetchAndCacheTrades";
import { useAddTrade } from "@hooks/Journal/useAddTrade";
import { Button, Stack } from '@mantine/core';
import { TbPlus } from "react-icons/tb";
import TradesLoadingState from "@journalComponents/JournalLoadingStates/TradesLoadingState";
import TradesErrorState from "@journalComponents/JournalLoadingStates/TradesErrorState";
import TradesEmptyState from "@journalComponents/JournalLoadingStates/TradesEmptyState";
import StyledActionButton from "@components/Common/StyledActionButton";

function JournalView() {
  const { isLoading, isError, trades } = useFetchAndCacheTrades();
  const { addTrade, isAddingTrade, newlyAddedTradeId } = useAddTrade();

  const onAddTrade = () => addTrade();

  if (isLoading) {
    return <TradesLoadingState />;
  }

  if (isError) {
    return <TradesErrorState />;
  }

  return (
    <Stack id="journalMainBody" className="bg-stone-50 p-4 border-t border-slate-200">
      {/* <FilterControl /> */}

      {trades?.length ? (
        <TradesGallery newlyAddedTradeId={newlyAddedTradeId} />
      ) : (
        <TradesEmptyState />
      )}

      <div className="flex justify-center">
        <StyledActionButton
          icon={<TbPlus size={20} />}
          onClick={onAddTrade}
          disabled={isAddingTrade}
          className="min-w-[280px]"
        >
          {isAddingTrade ? "Adding Trade..." : "+ Add a Trade"}
        </StyledActionButton>
      </div>
    </Stack>
  );
}

export default JournalView;