import "./App.css";
import { withErrorBoundary } from "react-error-boundary";
import GlobalAppGate from "@components/AppInitializer/GlobalAppGate";
import ErrorState from "@components/Common/LoadingStates/ErrorState";

// Named fallback component
function AppFallback({ error, resetErrorBoundary }) {
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

// Named App component
function App() {
  return <GlobalAppGate />;
}

// Named onError handler
function handleAppError(error, _info) {
  console.error("Unhandled app error:", error);
}

// Export wrapped component as a named constant
const AppWithErrorBoundary = withErrorBoundary(App, {
  FallbackComponent: AppFallback,
  onError: handleAppError,
});

export default AppWithErrorBoundary;
