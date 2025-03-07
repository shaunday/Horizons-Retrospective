import React, { useState } from "react";
import { Text, ActionIcon } from "@mantine/core";
import { useHover, useDisclosure } from "@mantine/hooks";
import { IconEdit } from "@tabler/icons-react";
import DefaultSelect from "./DefaultSelect";  // Import the new Select wrapper
import DataUpdateModal from "./DataUpdateModal";
import * as Constants from "@constants/journalConstants";
import { dataParser } from "./dataParser";

function ValueWrapper({ cellInfo, onValueChangeInitiated }) {
  const isOverview = onValueChangeInitiated === undefined;
  const { contentValue } = dataParser(cellInfo);
  const textRestrictionsExist = cellInfo[Constants.DATA_RESTRICTION_STRING]?.length > 0;

  const { opened: modalOpened, open: openModal, close: closeModal } = useDisclosure();
  const [dropdownOpened, setDropdownOpened] = useState(false);

  const { hovered, ref: wrapperRef } = useHover();

  const onEditRequested = () => openModal();

  const openDropdown = () => setDropdownOpened(true);
  const closeDropdown = () => setDropdownOpened(false);

  return (
    <div
      ref={wrapperRef}
      style={{ height: "40px", display: "flex", alignItems: "center" }}
    >
      {isOverview || (!hovered && !dropdownOpened) ? (
        <Text className="centered-text">{contentValue}</Text>
      ) : null}

      {!isOverview && (hovered || dropdownOpened) && textRestrictionsExist && (
        <DefaultSelect
          value={contentValue}
          onChange={onValueChangeInitiated}
          data={cellInfo[Constants.DATA_RESTRICTION_STRING]}
          opened={dropdownOpened}
          onDropdownOpen={openDropdown}
          onDropdownClose={closeDropdown}
        />
      )}

      {!isOverview && hovered && !textRestrictionsExist && (
        <>
          <ActionIcon variant="subtle" onClick={onEditRequested}>
            <IconEdit />
          </ActionIcon>
          <Text className="centered-text">{contentValue}</Text>
        </>
      )}

      <DataUpdateModal opened={modalOpened} onClose={closeModal} data={cellInfo} onSubmit={onValueChangeInitiated} />
    </div>
  );
}

export default React.memo(ValueWrapper);
