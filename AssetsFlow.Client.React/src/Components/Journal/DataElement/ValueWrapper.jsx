import React from "react";
import { Text, ActionIcon } from "@mantine/core";
import { useDisclosure } from "@mantine/hooks";
import { TbEdit } from "react-icons/tb";
import DefaultSelect from "./DefaultSelect";
import DataUpdateModal from "./DataUpdateModal";
import { dataElementContentParser } from "@services/dataElementContentParser";
import { useDelayedHover } from "@hooks/useDelayedHover";

function ValueWrapper({ cellInfo, onValueChangeInitiated }) {
  const isOverview = onValueChangeInitiated === undefined;
  const { contentValue, textRestrictions } = dataElementContentParser(cellInfo);
  const textRestrictionsExist = textRestrictions?.length > 0;

  const [modalOpened, { open: openModal, close: closeModal }] = useDisclosure(false);
  const [dropdownOpened, { open: openDropdown, close: closeDropdown }] = useDisclosure(false);

  const { delayedHover, ref: wrapperRef } = useDelayedHover(200); 

  return (
    <div ref={wrapperRef} style={{ height: "40px", display: "flex", alignItems: "center" }}>
      {isOverview || (!delayedHover && !dropdownOpened) ? (
        <Text
          className="centered-text"
          style={{
            borderBottom: !contentValue && !isOverview ? "1px dashed red" : "none",
          }}
        >
          {contentValue}
        </Text>
      ) : null}

      {!isOverview && (delayedHover || dropdownOpened) && textRestrictionsExist && (
        <DefaultSelect
          value={contentValue}
          onChange={onValueChangeInitiated}
          data={textRestrictions}
          opened={dropdownOpened ? "true" : "undefined"}
          onDropdownOpen={openDropdown}
          onDropdownClose={closeDropdown}
        />
      )}

      {!isOverview && delayedHover && !textRestrictionsExist && (
        <>
          <ActionIcon variant="subtle" onClick={openModal}>
            <TbEdit size={20} />
          </ActionIcon>
          <Text className="centered-text">{contentValue}</Text>
        </>
      )}

      <DataUpdateModal opened={modalOpened} onClose={closeModal} data={cellInfo} onSubmit={onValueChangeInitiated} />
    </div>
  );
}

export default React.memo(ValueWrapper);