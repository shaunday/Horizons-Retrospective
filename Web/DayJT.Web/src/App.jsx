import "./App.css";
import { useFetchAndCacheTrades } from "@hooks/useFetchAndCacheTrades";
import JournalContainer from "@views/JournalContainer";

function App() {
  const { prefetchTrades } = useFetchAndCacheTrades();

  useMemo(() => {
    prefetchTrades();
  }, []); // Ensures prefetchTrades is only called once

  return (
    <div id="vwrapper">
      <header id="header">Header placeholder</header>
      <main id="mainBody">
        <div className="flexChildCenter gotRightSideNeighbour">
          Metrics placeholder
        </div>
        <JournalContainer />
      </main>
      <footer id="footer">Footer placeholder</footer>
    </div>
  );
}

export default App;
