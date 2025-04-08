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
  maxWidth: "100%",
};

const containerStyle = {
  height: "40px",
  position: "relative",
};

const hoverAreaStyle = {
  height: "20px",
  position: "absolute",
  bottom: "-15px",
  width: "100%",
};

const iconStyle = {
  position: "absolute",
  bottom: "-5px",
  left: "50%",
  transform: "translateX(-50%)",
};

const contentContainerStyle = {
  maxWidth: "100%",
  borderRadius: "6px",
  background: "#fefefe",
};

function ValueWrapper({ cellInfo, onValueChangeInitiated }) {
  const isOverview = onValueChangeInitiated === undefined;
  const { contentValue } = dataElementContentParser(cellInfo);
  const [modalOpened, { open: openModal, close: closeModal }] = useDisclosure(false);
  const { delayedHover, ref: wrapperRef } = useDelayedHover(200);

  return (
    <div className="container-with-centered-content" style={containerStyle}>
      <div style={contentContainerStyle}>
        <Tooltip label={contentValue} disabled={contentValue.length < 20} withinPortal position="bottom">
          <Text style={textStyle}>{contentValue}</Text>
        </Tooltip>
      </div>

      <DataUpdateModal opened={modalOpened} onClose={closeModal} data={cellInfo} onSubmit={onValueChangeInitiated} />

      {!isOverview && (
        <div style={hoverAreaStyle} ref={wrapperRef}>
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