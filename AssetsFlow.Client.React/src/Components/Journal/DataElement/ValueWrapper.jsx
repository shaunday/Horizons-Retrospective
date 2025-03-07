import React, { useState } from "react";
import { ActionIcon, Text } from "@mantine/core";
import * as Constants from "@constants/journalConstants";
import { dataParser } from "./dataParser";
import DataUpdateModal from "./DataUpdateModal";
import ComboxBoxThingie from "../../ComboxBoxThingie";
import useHover from "@hooks/useHover";
import { IconEdit } from '@tabler/icons-react';

function ValueWrapper({ cellInfo, onValueChangeInitiated }) {
  const isOverview = onValueChangeInitiated === undefined;
  const { contentValue } = dataParser(cellInfo);
  const textRestrictionsExist = cellInfo[Constants.DATA_RESTRICTION_STRING]?.length > 0;
  const [modalOpened, setModalOpened] = useState(false);

  const { ref: wrapperRef, isHovered, setIsHovered } = useHover(); // Only tracking wrapper hover

  const onEditRequested = () => setModalOpened(true);

  const handleCloseModal = () => {
    setModalOpened(false);
    setIsHovered(false); // Force reset hover state
  };

  return (
    <div
      ref={wrapperRef}
      style={{ height: "40px", display: "flex", alignItems: "center" }}
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

      {/* If hovered and no restrictions, show ActionIcon + text */}
      {!isOverview && isHovered && !textRestrictionsExist && (
        <>
          <ActionIcon variant="subtle" onClick={onEditRequested}>
            <IconEdit />
          </ActionIcon>

          <Text className="centered-text">{contentValue}</Text>
        </>
      )}

      <DataUpdateModal opened={modalOpened} onClose={handleCloseModal} data={cellInfo} onSubmit={onValueChangeInitiated} />
    </div>
  );
}

export default React.memo(ValueWrapper);