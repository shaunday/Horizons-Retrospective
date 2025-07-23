import "./App.css";
import { withErrorBoundary } from "react-error-boundary";
import GlobalAppGate from "@components/GlobalAppGate";

// Fallback UI component for unhandled errors
function Fallback({ error, resetErrorBoundary }) {
  return (
    <div role="alert" className="p-8 text-center">
      <p className="text-lg font-semibold text-red-600">Something went wrong:</p>
      <pre className="text-sm text-red-500 mt-2">{error.message}</pre>
      <button
        className="mt-4 px-4 py-2 bg-red-600 text-white rounded"
        onClick={resetErrorBoundary}
      >
        Try again
      </button>
    </div>
  );
}

function App() {
  return <GlobalAppGate />;
}

export default withErrorBoundary(App, {
  FallbackComponent: Fallback,
  onError(error, info) {
    console.error("Unhandled app error:", error);
  },
});
