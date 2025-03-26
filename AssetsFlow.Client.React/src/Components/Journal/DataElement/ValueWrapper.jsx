import React from "react";
import { Text, Tooltip, ActionIcon } from "@mantine/core";
import { useDisclosure } from "@mantine/hooks";
import { TbEdit } from "react-icons/tb";
import DataUpdateModal from "./DataUpdateModal";
import { dataElementContentParser } from "@services/dataElementContentParser";
import { useDelayedHover } from "@hooks/useDelayedHover";

const textStyle = {
  whiteSpace: "nowrap",        // Prevent wrapping
  overflow: "hidden",          // Hide overflowed content
  textOverflow: "ellipsis",    // Show ellipsis when text is too long
  maxWidth: "100%",           // Constrain the width to force ellipsis (you can adjust this value)
};

const containerStyle = {
  height: "40px",
  display: "flex",
  alignItems: "center",
  width: "100%",                // Ensure the container takes up full width
  position: "relative",         // Set position relative for the reference area
};

const hoverAreaStyle = {
  height: "20px",               // Height of the hoverable area
  backgroundColor: "rgba(255, 235, 238, 0.5)",  // Light pink with transparency
  position: "absolute",         // Set position absolute
  bottom: "-15px",              // Move hover area 20px below the container (including the edit button offset)
  width: "100%",                // Full width of the parent container
};

const iconStyle = {
  position: "absolute",         // Position the icon absolutely inside the hover area
  bottom: "-5px",                // Move it 5px above the hover area bottom
  left: "50%",                  // Center it horizontally
  transform: "translateX(-50%)",// Centering adjustment
};

function ValueWrapper({ cellInfo, onValueChangeInitiated }) {
  const isOverview = onValueChangeInitiated === undefined;
  const { contentValue } = dataElementContentParser(cellInfo);
  const [modalOpened, { open: openModal, close: closeModal }] = useDisclosure(false);
  const { delayedHover, ref: wrapperRef } = useDelayedHover(200);

  return (
    <div style={containerStyle}>

      <div className="centered-text" style={{ padding: "5px 10px", maxWidth: "100%", borderRadius: "6px", background: "#fefefe" }}>
        <Tooltip label={contentValue} disabled={contentValue.length < 20} withinPortal position="bottom">
          <Text style={textStyle}>{contentValue}</Text>
        </Tooltip>
      </div>


      <DataUpdateModal opened={modalOpened} onClose={closeModal} data={cellInfo} onSubmit={onValueChangeInitiated} />

      {
        !isOverview &&
        <div style={hoverAreaStyle} ref={wrapperRef}>
          {delayedHover &&
            <ActionIcon
              variant="outline"
              onClick={openModal}
              style={iconStyle}
            >
              <TbEdit size={20} />
            </ActionIcon>}
        </div>
      }
    </div >
  );
}

export default React.memo(ValueWrapper);
