import React from "react";
import FilterControl from "@journalComponents/Filtering/FilterControl";
import TradesGallery from "@journalComponents/TradesGallery";
import { useFetchAndCacheTrades } from "@hooks/Journal/useFetchAndCacheTrades";
import { useAddTrade } from "@hooks/Journal/useAddTrade";
import { Stack } from "@mantine/core";
import { TbChartLine, TbPlus } from "react-icons/tb";
import LoadingState from "@components/Common/LoadingStates/LoadingState";
import ErrorState from "@components/Common/LoadingStates/ErrorState";
import EmptyState from "@components/Common/LoadingStates/EmptyState";
import StyledActionButton from "@components/Common/StyledActionButton";

function JournalView() {
  const { isLoading, isError, trades } = useFetchAndCacheTrades();
  const { addTrade, isAddingTrade, newlyAddedTradeId } = useAddTrade();

  const onAddTrade = () => addTrade();

  if (isLoading) {
    return (
      <LoadingState
        mainText="Loading your trading journal..."
        subText="Fetching trades and preparing your analysis"
      />
    );
  }

  if (isError) {
    return (
      <ErrorState
        mainText="Error loading trades"
        subText="Please try again later or check your connection"
      />
    );
  }

  if (!trades?.length) {
    return (
      <EmptyState
        mainText="No trades available"
        subText="Start by adding your first trade to begin tracking your performance"
      />
    );
  }

  return (
    <Stack
      id="journalMainBody"
      className="bg-stone-50 p-4 border-t border-slate-200"
    >
      {/* <FilterControl /> */}

      <TradesGallery newlyAddedTradeId={newlyAddedTradeId} />

      <div className="flex justify-center">
        <StyledActionButton
          icon={<TbPlus size={20} />}
          onClick={onAddTrade}
          disabled={isAddingTrade}
          className="min-w-[280px]"
        >
          {isAddingTrade ? "Adding Trade..." : "Add a Trade"}
        </StyledActionButton>
      </div>
    </Stack>
  );
}

export default JournalView;
