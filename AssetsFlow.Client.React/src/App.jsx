import "./App.css";
import { withErrorBoundary } from "react-error-boundary";
import GlobalAppGate from "@components/AppInitializer/GlobalAppGate";
import ErrorState from "@components/Common/LoadingStates/ErrorState";

// Fallback UI component for unhandled errors
function Fallback({ error, resetErrorBoundary }) {
  return (
    <ErrorState
      mainText="Something went wrong"
      subText={error.message}
      retryText="Try again"
      onRetry={resetErrorBoundary}
      color="red"
    />
  );
}

function App() {
  return <GlobalAppGate />;
}

export default withErrorBoundary(App, {
  FallbackComponent: Fallback,
  onError(error, _info) {
    console.error("Unhandled app error:", error);
  },
});
