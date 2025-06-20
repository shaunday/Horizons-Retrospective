import React from "react";
import { Text, Tooltip, ActionIcon } from "@mantine/core";
import { useDisclosure } from "@mantine/hooks";
import { TbEdit } from "react-icons/tb";
import DataUpdateModal from "./DataUpdateModal";
import { dataElementContentParser } from "@services/dataElementContentParser";
import { useDelayedHover } from "@hooks/useDelayedHover";

const textStyle = {
  whiteSpace: "nowrap",
  overflow: "hidden",
  textOverflow: "ellipsis",
};

const hoverAreaStyle = {
  position: "absolute",
  bottom: "-15px",
};

const iconStyle = {
  position: "absolute",
  bottom: "-5px",
  left: "50%",
  transform: "translateX(-50%)",
};

const contentContainerStyle = {
  flexShrink: 1,       // Allow it to shrink in row layout
  overflow: "hidden",  // Important to clip children
  borderRadius: "6px",
  background: "#fefefe",
};

function ValueWrapper({ cellInfo, onValueChangeInitiated }) {
  const isOverview = onValueChangeInitiated === undefined;
  const { contentValue } = dataElementContentParser(cellInfo);
  const [modalOpened, { open: openModal, close: closeModal }] = useDisclosure(false);
  const { delayedHover, ref: wrapperRef } = useDelayedHover(200);

  return (
    <div className="container-with-centered-content h-10 relative min-w-0">
      <div className="max-w-full" style={contentContainerStyle}>
        <Tooltip label={contentValue} disabled={contentValue.length < 20} withinPortal position="bottom">
          <Text className="max-w-full" style={textStyle}>{contentValue}</Text>
        </Tooltip>
      </div>

      <DataUpdateModal opened={modalOpened} onClose={closeModal} data={cellInfo} onSubmit={onValueChangeInitiated} />

      {!isOverview && (
        <div className="h-5 w-full" style={hoverAreaStyle} ref={wrapperRef}>
          {delayedHover && (
            <ActionIcon variant="outline" onClick={openModal} style={iconStyle}>
              <TbEdit size={20} />
            </ActionIcon>
          )}
        </div>
      )}
    </div>
  );
}

export default React.memo(ValueWrapper);