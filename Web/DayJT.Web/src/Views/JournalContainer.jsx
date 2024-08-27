import FilterControl from "@journalComponents/FilterControl";
import TradeComposite from "@journalComponents/TradeComposite";

export default function JournalContainer(trades) {
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
        onClick={addTradeInitiaded} //todo
      >
        Add a Trade
      </button>
    </div>
  );
}
