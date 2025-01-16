import "./App.css";
import { useEffect } from "react";
import { useFetchAndCacheTrades } from "@hooks/useFetchAndCacheTrades";
import JournalContainer from "@views/JournalContainer";

function App() {
  const { prefetchTrades } = useFetchAndCacheTrades();

  useEffect(() => {
    prefetchTrades();
  }, []); 

  return (
    <div id="vwrapper">
      <header>Header placeholder</header>
      <main>
        <div className="flexChildCenter gotRightSideNeighbour">
          Metrics placeholder
        </div>
        <JournalContainer />
      </main>
      <footer>Footer placeholder</footer>
    </div>
  );
}

export default App;
