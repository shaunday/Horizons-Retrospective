export const componentTypeStyles = {
  Header: {
    backgroundColor: "rgba(245, 245, 220, 0.8)", // Light beige with 80% opacity
    color: "#1F2937", // Dark text
  },
  Emotions: {
    backgroundColor: "rgba(255, 230, 204, 0.8)", // Soft pastel peach with 80% opacity
    color: "#991B1B", // Dark red
  },
  Thoughts: {
    backgroundColor: "rgba(209, 250, 229, 0.8)", // Light green with 80% opacity
    color: "#065F46", // Dark green
  },
  Technicals: {
    backgroundColor: "rgba(224, 242, 254, 0.8)", // Light blue with 80% opacity
    color: "#0284C7", // Dark blue
  },
  EntryLogic: {
    backgroundColor: "rgba(249, 250, 251, 0.8)", // Light neutral with 80% opacity
    color: "#6B7280", // Dark neutral gray
  },
  ExitLogic: {
    backgroundColor: "rgba(217, 228, 255, 0.8)", // Light violet with 80% opacity
    color: "#4338CA", // Dark violet
  },
  PriceRelated: {
    backgroundColor: "rgba(255, 230, 204, 0.8)", // Very light pastel peach with 80% opacity
    color: "#7A4B3D", // Muted brownish red
  },
  Risk: {
    backgroundColor: "rgba(230, 230, 255, 0.8)", // Light lavender with 80% opacity
    color: "#4C3B8C", // Dark lavender
  },
  Results: {
    backgroundColor: "rgba(209, 250, 229, 0.8)", // Light green with 80% opacity
    color: "#16A34A", // Dark green
  },
};

export const getComponentTypeStyles = (componentType) => {
  return componentTypeStyles[componentType] || {
    backgroundColor: "transparent",
    color: "#000000", // Default color if unknown
  };
};
