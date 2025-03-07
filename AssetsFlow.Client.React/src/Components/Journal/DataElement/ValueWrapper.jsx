import React from "react";
import { Text, ActionIcon } from "@mantine/core";
import { useHover, useDisclosure } from "@mantine/hooks";
import { IconEdit } from "@tabler/icons-react";
import DefaultSelect from "./DefaultSelect"; 
import DataUpdateModal from "./DataUpdateModal";
import * as Constants from "@constants/journalConstants";
import { dataParser } from "./dataParser";

function ValueWrapper({ cellInfo, onValueChangeInitiated }) {
  const isOverview = onValueChangeInitiated === undefined;
  const { contentValue } = dataParser(cellInfo);
  const textRestrictionsExist = cellInfo[Constants.DATA_RESTRICTION_STRING]?.length > 0;

  const [modalOpened, { open: openModal, close: closeModal }] = useDisclosure(false);
  const [dropdownOpened, { open: openDropdown, close: closeDropdown }] = useDisclosure(false);

  const { hovered, ref: wrapperRef } = useHover();

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
          opened={dropdownOpened ? "true" : "undefined"}
          onDropdownOpen={openDropdown} 
          onDropdownClose={closeDropdown} 
        />
      )}

      {!isOverview && hovered && !textRestrictionsExist && (
        <>
          <ActionIcon variant="subtle" onClick={openModal}> 
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