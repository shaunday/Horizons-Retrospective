import "./App.css";
import { useEffect } from "react";
import { useFetchAndCacheTrades } from "@hooks/Journal/useFetchAndCacheTrades";
import JournalView from "@views/JournalView";
import { withErrorBoundary } from "react-error-boundary";

// Fallback UI component for errors
function Fallback({ error, resetErrorBoundary }) {
  return (
    <div role="alert">
      <p>Something went wrong:</p>
      <pre style={{ color: "red" }}>{error.message}</pre>
      <button onClick={resetErrorBoundary}>Try again</button>
    </div>
  );
}

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
        <JournalView className="flexChildCenter" />
      </main>
      <footer>Footer placeholder</footer>
    </div>
  );
}

export default withErrorBoundary(App, {
  FallbackComponent: Fallback,
  onError(error, info) {
    console.error("Error occurred:", error);
  },
});
