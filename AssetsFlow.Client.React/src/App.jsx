import "./App.css";
import { useEffect } from "react";
import { useFetchAndCacheTrades } from "@hooks/useFetchAndCacheTrades";
import JournalContainer from "@views/JournalContainer";

function App() {
  // const { prefetchTrades } = useFetchAndCacheTrades();

  // useEffect(() => {
  //   prefetchTrades()
  //     .then(() => {
  //       console.log("Trades prefetched successfully");
  //     })
  //     .catch((error) => {
  //       console.error("Error prefetching trades:", error);
  //     });
  // }, []); //todo

  return (
    <div id="vwrapper">
      <header>Header placeholder</header>
      <main>
        {/* <div className="flexChildCenter gotRightSideNeighbour">
          Metrics placeholder
        </div> */}
        <JournalContainer className="flexChildCenter" />
      </main>
      <footer>Footer placeholder</footer>
    </div>
  );
}

export default App;
