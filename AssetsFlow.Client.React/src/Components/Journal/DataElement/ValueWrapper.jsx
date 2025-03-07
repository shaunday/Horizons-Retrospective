import React, { useState } from "react";
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

  const onEditRequested = () => setModalOpened(true);

  return (
    <div
      style={wrapperStyle}
      onMouseEnter={() => setIsHovered(true)}
      onMouseLeave={() => setIsHovered(false)}
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
            onMouseEnter={() => setIsEditHovered(true)}
            onMouseLeave={() => setIsEditHovered(false)}
          >
            ✏️
          </div>
          <Text className="centered-text">{contentValue}</Text>
        </>
      )}

      <DataUpdateModal
        opened={modalOpened}
        onClose={() => setModalOpened(false)}
        data={cellInfo}
        onSubmit={onValueChangeInitiated}
      />
    </div>
  );
}

export default React.memo(ValueWrapper);