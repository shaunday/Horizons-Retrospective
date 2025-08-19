import React, { useState } from "react";
import { Group, Stack } from "@mantine/core";
import { TbPlus } from "react-icons/tb";

import TradesGallery from "@journalComponents/TradesGallery";
import StyledActionButton from "@components/Common/StyledActionButton";
import EmptyState from "@components/Common/LoadingStates/EmptyState";
import { useFetchAndCacheTrades } from "@hooks/Journal/useFetchAndCacheTrades";
import { useAddTrade } from "@hooks/Journal/useAddTrade";

import FilterBar from "@components/Journal/Filtering/FilterBar";

function JournalView() {
  const [filters, setFilters] = useState([]);
  const { trades, loading } = useFetchAndCacheTrades(filters);
  const { addTrade, isAddingTrade, newlyAddedTradeId } = useAddTrade();

  return (
    <Stack id="journalMainBody" className="bg-stone-50 p-4 border-t border-slate-200" spacing="md">
      <Group position="apart" align="flex-start" spacing="sm" className="w-full">
        <div className="flex-1">
          <FilterBar onFiltersChange={setFilters} />
        </div>

        <StyledActionButton
          icon={<TbPlus size={20} />}
          onClick={addTrade}
          disabled={isAddingTrade}
          className="min-w-[200px] self-start mt-1 mr-3"
        >
          {isAddingTrade ? "Adding Trade..." : "Add a Trade"}
        </StyledActionButton>
      </Group>

      {loading ? (
        <div>Loading trades...</div>
      ) : trades?.length > 0 ? (
        <TradesGallery trades={trades} newlyAddedTradeId={newlyAddedTradeId} />
      ) : (
        <EmptyState
          mainText="No trades available"
          subText="Start by adding your first trade to begin tracking your performance"
        />
      )}
    </Stack>
  );
}

export default React.memo(JournalView);
