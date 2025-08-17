import React, { useRef } from "react";
import { Popover } from "@mantine/core";
import { useAtom } from "jotai";
import { openPopoverIdAtom } from "@state/popoverAtom";
import { dataElementContentParser } from "@services/dataElementContentParser";
import ValueDisplay from "./ValueDisplay";
import ValuePopover from "./ValuePopover";

function ValueWrapper({ cellInfo, onValueChangeInitiated }) {
  const isOverview = onValueChangeInitiated === undefined;
  const { contentValue, textRestrictions } = dataElementContentParser(cellInfo);

  const [openPopoverId, setOpenPopoverId] = useAtom(openPopoverIdAtom);
  const popoverId = cellInfo.id; // or any unique ID for this cell
  const preventCloseRef = useRef(false);

  const popoverOpened = openPopoverId === popoverId;

  const handleSubmit = (value, changeDetails) => {
    onValueChangeInitiated?.(value, changeDetails);
    setOpenPopoverId(null);
  };

  return (
    <Popover
      opened={popoverOpened}
      onChange={(open) => {
        if (!open && preventCloseRef.current) {
          preventCloseRef.current = false;
          return;
        }
        setOpenPopoverId(open ? popoverId : null);
      }}
      position="bottom"
      trapFocus={false}
      closeOnEscape
      withArrow
      arrowSize={12}
      shadow="md"
    >
      <Popover.Target>
        <ValueDisplay
          contentValue={contentValue}
          isOverview={isOverview}
          disallowTooltip={popoverOpened}
          onClick={() => !isOverview && setOpenPopoverId(popoverId)}
        />
      </Popover.Target>

      <Popover.Dropdown
        onMouseDown={() => {
          preventCloseRef.current = true;
        }}
        className="bg-neutral-50 border border-blue-50 rounded-lg p-3 shadow-md"
      >
        <ValuePopover
          contentValue={contentValue}
          textRestrictions={textRestrictions}
          onSubmit={handleSubmit}
        />
      </Popover.Dropdown>
    </Popover>
  );
}

export default React.memo(ValueWrapper);
