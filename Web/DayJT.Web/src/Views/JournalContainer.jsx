import FilterControl from "@journalComponents/FilterControl";
import TradeComposite from "@journalComponents/TradeComposite";
import { useTrades } from "@hooks/useTrades";

export default function JournalContainer() {
  const { trades } = useTrades();

  return (
    <div id="journalMainBody">
      <FilterControl />
      <ul>
        {trades.map((composite) => (
          <li key={composite.id}>
            <TradeComposite tradeComposite={composite} />
          </li>
        ))}
      </ul>
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
