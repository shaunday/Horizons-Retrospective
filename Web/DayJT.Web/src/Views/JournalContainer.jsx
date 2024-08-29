import { useState } from "react";
import FilterControl from "@journalComponents/FilterControl";
import TradesContainer from "@journalComponents/TradesContainer";
import { useTrades } from "@hooks/useTrades";

export default function JournalContainer() {
  const [isAddTrade, setIsAddTrade] = useState(false);

  const { trades, addTradeData, isAddingTrade } = useTrades(
    IDS,
    isAddTrade,
    () => setIsAddTrade(false) // onTradeAdded callback
  );

  return (
    <div id="journalMainBody">
      <FilterControl />
      <TradesContainer trades={trades} />
      <button
        className="button-38"
        type="button"
        style={{ justifyContent: "center" }}
        onClick={() => setIsAddTrade(true)}
      >
        Add a Trade
      </button>
    </div>
  );
}
