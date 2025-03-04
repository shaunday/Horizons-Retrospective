import React, { useState, useCallback } from "react";
import TradeExpanded from "./TradeExpanded";
import TradeCollapsed from "./TradeCollapsed";

const styles = {
  container: {
    display: "flex",
    alignItems: "center",
    height: "100%",
  },
  toggleButton: {
    cursor: "pointer",
    height: "100%",
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
  },
};

function TradeWrapper({ tradeId }) {
  const [isCollapsed, setIsCollapsed] = useState(true);

  const toggleExpand = useCallback(() => {
    setIsCollapsed((prev) => !prev);
  }, []);

  return (
    <div style={styles.container}>
      <div onClick={toggleExpand} style={styles.toggleButton}>
        {isCollapsed ? "🔽" : "🔼"}
      </div>
      {isCollapsed ? (
        <TradeCollapsed tradeId={tradeId} />
      ) : (
        <TradeExpanded tradeId={tradeId} />
      )}
    </div>
  );
}

export default React.memo(TradeWrapper);
