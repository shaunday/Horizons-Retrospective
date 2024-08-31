import FilterControl from "@journalComponents/FilterControl";
import TradesContainer from "@journalComponents/TradesContainer";
import { useAddTrade } from "@hooks/useAddTrade";

export default function JournalContainer() {
  const { addTrade } = useAddTrade();

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
