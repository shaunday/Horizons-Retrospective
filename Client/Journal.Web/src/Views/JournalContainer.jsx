import FilterControl from "@journalComponents/FilterControl";
import TradesContainer from "@journalComponents/TradesContainer";
import { useAddTrade } from "@hooks/useAddTrade";

function JournalContainer() {
  const { tradesQuery } = useFetchAndCacheTrades();
  const { isLoading, isError, trades } = tradesQuery;
  const { addTrade } = useAddTrade();

  if (isLoading) {
    return <div>Loading trades...</div>;
  }

  if (isError) {
    return <div>Error fetching trades. Please try again later.</div>;
  }

  if (!trades || trades.length === 0) {
    return <div>No trades available.</div>;
  }

  return (
    <div id="journalMainBody">
      <FilterControl />
      <TradesContainer />
      <button
        className="button-38"
        type="button"
        style={{ justifyContent: "center" }}
        onClick={addTrade}
      >
        Add a Trade
      </button>
    </div>
  );
}

export default JournalContainer;
