import React from "react";
import { Stack } from "@mantine/core";
import { TbPlus } from "react-icons/tb";

import TradesGallery from "@journalComponents/TradesGallery";
import StyledActionButton from "@components/Common/StyledActionButton";
import EmptyState from "@components/Common/LoadingStates/EmptyState";
import { useFetchAndCacheTrades } from "@hooks/Journal/useFetchAndCacheTrades";
import { useAddTrade } from "@hooks/Journal/useAddTrade";

function JournalView() {
  const { trades } = useFetchAndCacheTrades(); // Already loaded via GlobalAppGate
  const { addTrade, isAddingTrade, newlyAddedTradeId } = useAddTrade();

  return (
    <Stack id="journalMainBody" className="bg-stone-50 p-4 border-t border-slate-200">
      {trades?.length > 0 ? (
        <TradesGallery newlyAddedTradeId={newlyAddedTradeId} />
      ) : (
        <EmptyState
          mainText="No trades available"
          subText="Start by adding your first trade to begin tracking your performance"
        />
      )}

      <div className="flex justify-center">
        <StyledActionButton
          icon={<TbPlus size={20} />}
          onClick={addTrade}
          disabled={isAddingTrade}
          className="min-w-[280px]"
        >
          {isAddingTrade ? "Adding Trade..." : "Add a Trade"}
        </StyledActionButton>
      </div>
    </Stack>
  );
}

export default React.memo(JournalView);
