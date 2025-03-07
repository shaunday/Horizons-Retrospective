import React, { useState } from "react";
import { ActionIcon, Text } from "@mantine/core";
import { useHover } from '@mantine/hooks';
import * as Constants from "@constants/journalConstants";
import { dataParser } from "./dataParser";
import DataUpdateModal from "./DataUpdateModal";
import ComboxBoxThingie from "../../ComboxBoxThingie";
import { IconEdit } from '@tabler/icons-react';

function ValueWrapper({ cellInfo, onValueChangeInitiated }) {
  const isOverview = onValueChangeInitiated === undefined;
  const { contentValue } = dataParser(cellInfo);
  const textRestrictionsExist = cellInfo[Constants.DATA_RESTRICTION_STRING]?.length > 0;
  const [modalOpened, setModalOpened] = useState(false);

  const { ref: wrapperRef, hovered : isHovered } = useHover(); 

  const onEditRequested = () => setModalOpened(true);

  const handleCloseModal = () => {
    setModalOpened(false);
  };

  return (
    <div
      ref={wrapperRef}
      style={{ height: "40px", display: "flex", alignItems: "center" }}
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