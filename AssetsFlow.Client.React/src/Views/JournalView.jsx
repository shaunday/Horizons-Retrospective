import FilterControl from "@journalComponents/Filtering/FilterControl";
import PnLLineChart from "@components/PnLLineChart"
import TradesContainer from "@journalComponents/TradesContainer";
import { useFetchAndCacheTrades } from "@hooks/useFetchAndCacheTrades";
import { useAddTrade } from "@hooks/useAddTrade";

function JournalView() {
  const { isLoading, isError, trades } = useFetchAndCacheTrades();
  const { addTrade, isAddingTrade } = useAddTrade();


  if (isLoading) {
    return <div>Loading trades...</div>;
  }

  if (isError) {
    return <div>Error fetching trades. Please try again later.</div>;
  }

  if (!trades || trades.length === 0) {
    return <div>No trades available.</div>;
  }

  const onAddTrade = ()=> {
    addTrade();
  }

  return (
    <div id="journalMainBody">
      {/* <FilterControl />
      <PnLLineChart /> */}
      <TradesContainer />
      <button
        className="button-38"
        type="button"
        style={{ justifyContent: "center", marginTop: "auto" }}
        onClick={onAddTrade}
        disabled={isAddingTrade} // ✅ Disable button while adding trade
      >
        {isAddingTrade ? "Adding Trade..." : "Add a Trade"}
      </button>
    </div>
  );
}

export default JournalView;
