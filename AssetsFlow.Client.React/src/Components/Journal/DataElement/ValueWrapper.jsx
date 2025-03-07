import React, { useState, useRef, useEffect } from "react";
import { Text } from "@mantine/core";
import * as Constants from "@constants/journalConstants";
import { dataParser } from "./dataParser";
import DataUpdateModal from "./DataUpdateModal";
import ComboxBoxThingie from "../../ComboxBoxThingie";

// Extracted styles
const wrapperStyle = {
  height: "40px",
  display: "flex",
  alignItems: "center",
  position: "relative",
};

const editIconStyle = {
  marginRight: 8,
  padding: "4px 6px",
  border: "1px solid #ccc",
  borderRadius: "4px",
  cursor: "pointer",
  display: "inline-flex",
  alignItems: "center",
  justifyContent: "center",
  fontSize: "14px",
  backgroundColor: "#f8f8f8",
  transition: "background-color 0.2s ease",
};

const editIconHoverStyle = {
  backgroundColor: "#e0e0e0",
};

function ValueWrapper({ cellInfo, onValueChangeInitiated }) {
  const isOverview = onValueChangeInitiated === undefined;
  const { contentValue } = dataParser(cellInfo);
  const textRestrictionsExist = cellInfo[Constants.DATA_RESTRICTION_STRING]?.length > 0;
  const [modalOpened, setModalOpened] = useState(false);
  const [isHovered, setIsHovered] = useState(false);
  const [isEditHovered, setIsEditHovered] = useState(false);

  const wrapperRef = useRef(null);

  const onEditRequested = () => setModalOpened(true);

  const handleCloseModal = () => {
    setModalOpened(false);
    setIsHovered(false); // Force reset hover state
  };

  // Force reset hover state if the mouse leaves too quickly
  useEffect(() => {
    const handlePointerMove = (event) => {
      if (isHovered && wrapperRef.current && !wrapperRef.current.contains(event.target)) {
        setIsHovered(false);
      }
    };

    document.addEventListener("pointermove", handlePointerMove);
    return () => document.removeEventListener("pointermove", handlePointerMove);
  }, [isHovered]);

  return (
    <div
      ref={wrapperRef}
      style={wrapperStyle}
      onPointerEnter={() => setIsHovered(true)}
      onPointerLeave={() => setIsHovered(false)}
      onBlur={() => setIsHovered(false)} // Reset when focus is lost
      tabIndex={0} // Makes the div focusable for onBlur
    >
      {/* Always show text if it's an overview OR not hovered */}
      {isOverview || !isHovered ? <Text className="centered-text">{contentValue}</Text> : null}

      {/* If hovered and has restrictions, show ComboxBoxThingie */}
      {!isOverview && isHovered && textRestrictionsExist && (
        <ComboxBoxThingie
          selected={contentValue}
          options={cellInfo[Constants.DATA_RESTRICTION_STRING]}
          onSelect={onValueChangeInitiated}
        />
      )}

      {/* If hovered and no restrictions, show Edit button + text */}
      {!isOverview && isHovered && !textRestrictionsExist && (
        <>
          <div
            style={{ ...editIconStyle, ...(isEditHovered ? editIconHoverStyle : {}) }}
            onClick={onEditRequested}
            onPointerEnter={() => setIsEditHovered(true)}
            onPointerLeave={() => setIsEditHovered(false)}
          >
            ✏️
          </div>
          <Text className="centered-text">{contentValue}</Text>
        </>
      )}

      <DataUpdateModal opened={modalOpened} onClose={handleCloseModal} data={cellInfo} onSubmit={onValueChangeInitiated} />
    </div>
  );
}

export default React.memo(ValueWrapper);