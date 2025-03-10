const opacity = 0.3;

export const componentTypeStyles = {
  Header: {
    backgroundColor: `rgba(245, 245, 220, ${opacity})`, // Light beige with variable opacity
    color: "#1F2937", // Dark text
  },
  Emotions: {
    backgroundColor: `rgba(255, 230, 204, ${opacity})`, // Soft pastel peach with variable opacity
    color: "#991B1B", // Dark red
  },
  Thoughts: {
    backgroundColor: `rgba(209, 250, 229, ${opacity})`, // Light green with variable opacity
    color: "#065F46", // Dark green
  },
  Technicals: {
    backgroundColor: `rgba(224, 242, 254, ${opacity})`, // Light blue with variable opacity
    color: "#0284C7", // Dark blue
  },
  EntryLogic: {
    backgroundColor: `rgba(249, 250, 251, ${opacity})`, // Light neutral with variable opacity
    color: "#6B7280", // Dark neutral gray
  },
  ExitLogic: {
    backgroundColor: `rgba(217, 228, 255, ${opacity})`, // Light violet with variable opacity
    color: "#4338CA", // Dark violet
  },
  PriceRelated: {
    backgroundColor: `rgba(247, 226, 164, ${opacity})`, // Soft pastel yellow with variable opacity
    color: "#9A6A2E", // Muted dark yellow
  },
  Risk: {
    backgroundColor: `rgba(255, 204, 204, ${opacity})`, // Soft pastel peach with variable opacity
    color: "#9E3D3D", // Muted dark red
  },
  Results: {
    backgroundColor: `rgba(209, 250, 229, ${opacity})`, // Light green with variable opacity
    color: "#16A34A", // Dark green
  },
};

export const getComponentTypeStyles = (componentType) => {
  return componentTypeStyles[componentType] || {
    backgroundColor: "transparent",
    color: "#000000", // Default color if unknown
  };
};
