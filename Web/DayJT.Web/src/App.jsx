import "./App.css";
import { useFetchAndCacheTrades } from "@hooks/useFetchAndCacheTrades";
import JournalContainer from "@views/JournalContainer";

function App() {
  const tradesQuery = useFetchAndCacheTrades();

  return (
    <div id="vwrapper">
      <div id="header">Header placeholder</div>
      <div id="mainBody">
        <div className="flexChildCenter gotRightSideNeighbour">
          Metrics placeholder
        </div>
        <JournalContainer tradesQuery={tradesQuery} />
      </div>
      <div id="footer">Footer placeholder</div>
    </div>
  );
}

export default App;
