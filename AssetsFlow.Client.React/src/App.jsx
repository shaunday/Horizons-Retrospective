import "./App.css";
import JournalView from "@views/JournalView";
import { withErrorBoundary } from "react-error-boundary";
import Header from "@views/Header";
import Footer from "@views/Footer";
import DemoLoginGate from "@components/DemoLoginGate";

// Fallback UI component for errors
function Fallback({ error, resetErrorBoundary }) {
  return (
    <div role="alert" className="p-4">
      <p>Something went wrong:</p>
      <pre className="text-red-600">{error.message}</pre>
      <button
        onClick={resetErrorBoundary}
        className="mt-2 px-4 py-2 border rounded bg-gray-100 hover:bg-gray-200"
      >
        Try again
      </button>
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
    <DemoLoginGate>
      <div id="vwrapper">
        <Header />
        <main>
          <JournalView />
        </main>
        <Footer />
      </div>
    </DemoLoginGate>
  );
}

export default withErrorBoundary(App, {
  FallbackComponent: Fallback,
  onError(error, info) {
    console.error("Error occurred:", error, info);
  },
});