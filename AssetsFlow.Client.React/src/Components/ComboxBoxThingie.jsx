import React from "react";

const styles = {
  select: {
    padding: "5px",
    borderRadius: "4px",
    border: "1px solid #ccc",
    background: "white",
    cursor: "pointer",
  },
};

function SelectionForDataElement({ selected, options, onSelect }) {

  const handleChange = (event) => {
    const value = event.target.value;
    onSelect(value); 
  };

  return (
    <select
      value={selected}
      onChange={handleChange}
      style={styles.select}
    >
      <option value="" disabled>
        {selected === "" ? "Select..." : selected} {/* Conditionally show the placeholder */}
      </option>
      {options.map((option, index) => (
        <option key={index} value={option}>
          {option}
        </option>
      ))}
    </select>
  );
}

export default React.memo(SelectionForDataElement);
