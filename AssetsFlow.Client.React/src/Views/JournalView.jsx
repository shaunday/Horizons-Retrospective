import FilterControl from "@journalComponents/Filtering/FilterControl";
import PnLLineChart from "@components/PnLLineChart";
import TradesContainer from "@journalComponents/TradesContainer";
import { useFetchAndCacheTrades } from "@hooks/useFetchAndCacheTrades";
import { useAddTrade } from "@hooks/useAddTrade";
import { Button } from '@mantine/core';

const buttonStyles = { marginTop: "5px", marginRight: "auto", marginLeft: "auto" };

function JournalView() {
  const { isLoading, isError, trades } = useFetchAndCacheTrades();
  const { addTrade, isAddingTrade } = useAddTrade();

  const onAddTrade = () => addTrade(); 

  if (isLoading) return <div>Loading trades...</div>;
  if (isError) return <div>Error fetching trades. Please try again later.</div>;
  if (!trades?.length) return <div>No trades available.</div>; 

  return (
    <div id="journalMainBody">
      {/* <FilterControl />
      <PnLLineChart /> */}
      <TradesContainer />
      <Button
        onClick={onAddTrade}
        style={buttonStyles} 
        disabled={isAddingTrade} 
      >
        {isAddingTrade ? "Adding Trade..." : "Add a Trade"}
      </Button>
    </div>
  );
}

export default JournalView;
